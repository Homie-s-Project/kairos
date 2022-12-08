using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Kairos.API.Context;
using Kairos.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Kairos.API.Controllers;

[Route(("studies"))]
public class StudiesController : SecurityController
{
    private const int MinutesMinimumHeatbeat = 2;
    
    private readonly IMemoryCache _memoryCache;
    private readonly KairosContext _context;

    public StudiesController(IMemoryCache memoryCache, KairosContext context)
    {
        _memoryCache = memoryCache;
        _context = context;
    }

    
    [HttpPost("start")]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErrorMessage))]
    public async Task<ActionResult> StartStudies()
    {
        var user = (User) HttpContext.Items["User"];

        // Si l'utilisateur n'est pas connecté,
        if (user == null)
        {
            return Forbid("Not access");
        }

        var liveStudy = (LiveStudies) _memoryCache.Get(user.UserId);
        if (liveStudy != null)
        {
            // S'il a été actualisé il y a moins de 2 {MinutesMinimumHeatbeat} minutes
            if (liveStudy.LastRefresh.AddMinutes(MinutesMinimumHeatbeat) > DateTime.UtcNow)
            {
                return NotFound(new ErrorMessage("This user has already an session started, please end the last one before.", StatusCodes.Status406NotAcceptable));
            }
            
            // S'il a été actualisé il y a plus de 2 {MinutesMinimumHeatbeat} minutes
            if (liveStudy.LastRefresh.AddMinutes(MinutesMinimumHeatbeat) < DateTime.UtcNow)
            {
                // On supprime l'ancienne session
                _memoryCache.Remove(user.UserId);
                
                // On la sauvegarde dans la base de données
                var study = new Studies(System.Guid.NewGuid().ToString(),(liveStudy.LastRefresh - liveStudy.StartTime).TotalSeconds.ToString(CultureInfo.InvariantCulture), liveStudy.StartTime, 2);
                _context.Studies.Add(study);

                return NotFound(new ErrorMessage("This user has already an session started, this session has been stoped and saved.", StatusCodes.Status406NotAcceptable));
            }
        }
        
        
        var startStudy = new LiveStudies
        {
            UserId = user.UserId,
            TimePlanned = 2,
            Labels = new List<LabelDto>()
        };

        _memoryCache.Set(user.UserId, startStudy);
        
        return Ok(new ErrorMessage("Session started", StatusCodes.Status200OK));
    }
    
    [HttpPost("stop")]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErrorMessage))]
    public async Task<ActionResult> StopStudies()
    {
        var user = (User) HttpContext.Items["User"];

        // Si l'utilisateur n'est pas connecté,
        if (user == null)
        {
            return Forbid("Not access");
        }

        var liveStudy = _memoryCache.Get(user.UserId);
        if (liveStudy == null)
        {
            return NotFound(new ErrorMessage("This user has no session to stop.", StatusCodes.Status406NotAcceptable));
        }
        
        _memoryCache.Remove(user.UserId);
        
        return Ok(new ErrorMessage("Session ended", StatusCodes.Status200OK));
    }


    /// <summary>
    /// send heatbeat to check if user is still studying
    /// </summary>
    /// <returns></returns>
    [HttpPost("heartbeat")]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> HeartBeat()
    {
        var user = (User) HttpContext.Items["User"];

        // Si l'utilisateur n'est pas connecté,
        if (user == null)
        {
            return Forbid("Not access");
        }

        var liveStudy = (LiveStudies) _memoryCache.Get(user.UserId);
        if (liveStudy == null)
        {
            return NotFound(new ErrorMessage("This user has no session to give an heatbeat.", StatusCodes.Status406NotAcceptable));
        }
        
        // S'il a été actualisé il y a plus de 2 {MinutesMinimumHeatbeat} minutes
        if (liveStudy.LastRefresh.AddMinutes(MinutesMinimumHeatbeat) < DateTime.UtcNow)
        {
            // On supprime l'ancienne session
            _memoryCache.Remove(user.UserId);
                
            // On la sauvegarde dans la base de données
            var study = new Studies(System.Guid.NewGuid().ToString(),(liveStudy.LastRefresh - liveStudy.StartTime).TotalSeconds.ToString(CultureInfo.InvariantCulture), liveStudy.StartTime, 2);
            _context.Studies.Add(study);

            return NotFound(new ErrorMessage("The last session was unactive for more than " + MinutesMinimumHeatbeat + " minutes. The session has been saved.", StatusCodes.Status406NotAcceptable));
        }

        liveStudy.LastRefresh = DateTime.UtcNow;
        _memoryCache.Set(user.UserId, liveStudy);

        return Ok(new ErrorMessage("Heartbeat sent", StatusCodes.Status200OK));
    }

    /// <summary>
    /// return the studies with the studies number
    /// </summary>
    /// <param name="studiesId">the studies number</param>
    /// <returns></returns>
    [HttpGet("/{studiesId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudiesDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult GetStudies(string studiesId)
    {
        var userConterxt = (User) HttpContext.Items["User"]; 
        if (userConterxt == null)
        { 
            return Forbid("Not access");
        }
                
        if (string.IsNullOrEmpty(studiesId))
        {
            return BadRequest(new ErrorMessage("Studies id not specified", StatusCodes.Status400BadRequest));
        }

        int studiesIdParsed;
        bool isParsed = int.TryParse(studiesId, out studiesIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Studies id is not valid", StatusCodes.Status400BadRequest));
        }

        var studies = _context.Studies
            .Include(s => s.Labels)
            .FirstOrDefault(s => s.StudiesId == studiesIdParsed &&
                                 (s.Group.Users.FirstOrDefault(u => u.UserId == userConterxt.UserId) != null ||
                                  s.Group.OwnerId == userConterxt.UserId));
        
        if (studies == null)
        {
            return NotFound(new ErrorMessage("Studies not found", StatusCodes.Status404NotFound));
        }

        _context.Entry(studies).Collection(s => s.Labels).Load();
        return Ok(new StudiesDto(studies, true));
    }

    /// <summary>
    /// return the number of hours per days of studies
    /// </summary>
    [HttpGet("lastWeek/hoursStudied")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LastWeekHoursData))]
    public IActionResult LastWeekWork()
    {
        
        var userConterxt = (User) HttpContext.Items["User"];
        if (userConterxt == null)
        {
            return Forbid("Not access");
        }
        
        var studiesLastWeeks = _context.Studies
            .Where(s =>
                (s.Group.Users.FirstOrDefault(u => u.UserId == userConterxt.UserId) != null ||
                 s.Group.OwnerId == userConterxt.UserId) &&
                s.StudiesCreatedDate >= DateTime.Now.AddDays(-7))
            .ToList();

        Dictionary<string, float> data = new Dictionary<string, float>();
        studiesLastWeeks.ForEach((s) =>
        {
            int studiedTime;
            bool isParsed = int.TryParse(s.StudiesTime, out studiedTime);
            if (isParsed)
            {
                data.Add(s.StudiesCreatedDate.DayOfWeek.ToString(), (float) studiedTime / 3_600);
            }
        });

        return Ok(new LastWeekHoursData(data));
    }

    /// <summary>
    /// return the last week hours of studies per label
    /// </summary>
    [HttpGet("lastWeek/hoursPerLabel")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LastWeekWorkPerLabel))]
    public IActionResult LastWeekHoursPerLabel()
    {
        var userConterxt = (User) HttpContext.Items["User"];
        if (userConterxt == null)
        {
            return Forbid("Not access");
        }
        
        var studiesLastWeeks = _context.Studies
            .Where(s => 
                s.Group.Users.FirstOrDefault(u => u.UserId == userConterxt.UserId) != null || 
                s.Group.OwnerId == userConterxt.UserId &&
                s.StudiesCreatedDate >= DateTime.Now.AddDays(-7))
            .Include(s => s.Labels)
            .ToList();

        if (studiesLastWeeks.Count == 0)
        {
            return NotFound(new ErrorMessage("Studies not found", StatusCodes.Status404NotFound));
        }

        studiesLastWeeks.ForEach((s) =>
        {
            _context.Entry(s).Collection(s => s.Labels).Load();
        });

        var studiesDtos = studiesLastWeeks.Select(s => new StudiesDto(s, true)).ToList();
        Dictionary<string, float> data = new Dictionary<string, float>();
        
        studiesDtos.ForEach((s) => {
            if (s.StudiesLabels== null)
            {
                return;
            }
            
            var studiesLabel = s.StudiesLabels.ToList();
            studiesLabel.ForEach((l) =>
            {
                int studiedTime;
                bool isParsed = int.TryParse(s.StudiesTime, out studiedTime);
                if (isParsed)
                {
                    if (data.ContainsKey(l.LabelTitle))
                    {
                        data[l.LabelTitle] += (float) studiedTime / 3_600;
                    }
                    else
                    {
                        data.Add(l.LabelTitle, (float) studiedTime / 3_600);
                    }
                }
            });
        });

        return Ok(new LastWeekWorkPerLabel(data));
    }

    /// <summary>
    /// return the rate of studying last week
    /// </summary>
    [HttpGet("lastWeek/rate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public IActionResult LastWeekRate()
    {
        var userConterxt = (User) HttpContext.Items["User"];
        if (userConterxt == null)
        {
            return Forbid("Not access");
        }
        
        var studiesLastWeeks = _context.Studies
            .Where(s => 
                s.Group.Users.FirstOrDefault(u => u.UserId == userConterxt.UserId) != null || 
                s.Group.OwnerId == userConterxt.UserId &&
                s.StudiesCreatedDate >= DateTime.Now.AddDays(-7))
            .ToList();
        
        
        var studiesBeforeLastWeek = _context.Studies
            .Where(s => 
                s.Group.Users.FirstOrDefault(u => u.UserId == userConterxt.UserId) != null || 
                s.Group.OwnerId == userConterxt.UserId &&
                s.StudiesCreatedDate >= DateTime.Now.AddDays(-14) &&
                s.StudiesCreatedDate < DateTime.Now.AddDays(-7))
            .ToList();
        
        var studiesLastWeeksTime = studiesLastWeeks.Sum(s => int.Parse(s.StudiesTime));
        var studiesBeforeLastWeekTime = studiesBeforeLastWeek.Sum(s => int.Parse(s.StudiesTime));

        int rate = (studiesLastWeeksTime * 100) / studiesBeforeLastWeekTime;
        rate -= 100;

        return Ok(rate);
    }

    /// <summary>
    /// refresh memory cache
    /// </summary>
    /// <param name="encryptServiceId">the encrypted service id from the oauth provider (google, microsoft)</param>
    private void RefreshMemory(string encryptServiceId)
    {
        // Entrée du cache qui est égale à 5min.
        var cacheEntryOption = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        _memoryCache.Set(encryptServiceId, DateTime.Now, cacheEntryOption);
    }

    private int GenerateNumberBetween(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    }
}

public class LastWeekWorkPerLabel
{
    public Dictionary<string, float>.KeyCollection Labels { get; set; }
    public Dictionary<string, float>.ValueCollection Hours { get; set; }

    public LastWeekWorkPerLabel(Dictionary<string, float> data)
    {
        Labels = data.Keys;
        Hours = data.Values;
    }
}

public class LastWeekHoursData
{
    public Dictionary<string, float>.KeyCollection DayOfWeek { get; set; }
    public Dictionary<string, float>.ValueCollection Hours { get; set; }

    public LastWeekHoursData(Dictionary<string, float> data)
    {
        DayOfWeek = data.Keys;
        Hours = data.Values;
    }
}