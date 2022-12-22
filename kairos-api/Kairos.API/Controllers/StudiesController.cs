using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Kairos.API.Context;
using Kairos.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    /// <summary>
    /// Permet de commencer une session de travail
    /// </summary>
    /// <returns></returns>
    [HttpPost("start")]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErrorMessage))]
    public async Task<ActionResult> StartStudies([FromForm] string timer, [FromForm] string labelsId)
    {
        var userContext = (User) HttpContext.Items["User"];

        // Si l'utilisateur n'est pas connecté,
        if (userContext == null)
        {
            return Forbid("Not access");
        }

        var liveStudy = (LiveStudies) _memoryCache.Get(userContext.UserId);
        if (liveStudy != null)
        {
            // S'il a été actualisé il y a moins de 2 {MinutesMinimumHeatbeat} minutes
            if (liveStudy.LastRefresh.AddMinutes(MinutesMinimumHeatbeat) > DateTime.UtcNow)
            {
                return NotFound(new ErrorMessage(
                    "This user has already an session started, please end the last one before.",
                    StatusCodes.Status406NotAcceptable));
            }

            // S'il a été actualisé il y a plus de 2 {MinutesMinimumHeatbeat} minutes
            if (liveStudy.LastRefresh.AddMinutes(MinutesMinimumHeatbeat) < DateTime.UtcNow)
            {
                // On supprime l'ancienne session
                _memoryCache.Remove(userContext.UserId);

                // On la sauvegarde dans la base de données
                var study = new Studies(Guid.NewGuid().ToString(),
                    ((int) (liveStudy.LastRefresh - liveStudy.StartTime).TotalSeconds).ToString(CultureInfo
                        .InvariantCulture),
                    liveStudy.StartTime, GetPersonalGroup());
                
                study.Labels = new List<Label>();
                liveStudy.Labels.ForEach(label =>
                {
                    var labelDb = _context.Labels.FirstOrDefault(l => l.LabelId == label.LabelId && l.UserId == userContext.UserId);
                    if (labelDb != null)
                    {
                        study.Labels.Add(labelDb);
                    }
                });

                _context.Studies.Add(study);
                await _context.SaveChangesAsync();

                return NotFound(new ErrorMessage(
                    "This user has already an session started, this session has been stoped and saved.",
                    StatusCodes.Status406NotAcceptable));
            }
        }

        if (string.IsNullOrEmpty(timer))
        {
            return BadRequest(new ErrorMessage("The timer is required.", StatusCodes.Status406NotAcceptable));
        }
        
        var timerParsed = Int32.TryParse(timer, out var timerInt);
        if (!timerParsed)
        {
            return BadRequest(new ErrorMessage("The timer is not a number.", StatusCodes.Status500InternalServerError));
        }

        if (!string.IsNullOrEmpty(labelsId))
        {
            var labels = labelsId.Split(',');
            List<LabelDto> labelsForStudy = new List<LabelDto>();

            foreach (var label in labels)
            {
                var labelsParsed = Int32.TryParse(label, out var labelInt);
                if (!labelsParsed)
                {
                    return BadRequest(new ErrorMessage("The timer is not a number.",
                        StatusCodes.Status500InternalServerError));
                }

                var labelDb = await _context.Labels.FirstOrDefaultAsync(l => l.LabelId == labelInt && l.UserId == userContext.UserId);
                if (labelDb == null)
                {
                    return Unauthorized(new ErrorMessage("This user is not allowed to use this label.",
                        StatusCodes.Status401Unauthorized));
                }

                labelsForStudy.Add(new LabelDto(labelDb, false));
            }

            var startStudyWithLabel = new LiveStudies
            {
                UserId = userContext.UserId,
                TimePlanned = timerInt,
                Labels = labelsForStudy,
                StartTime = DateTime.UtcNow,
                LastRefresh = DateTime.UtcNow
            };

            _memoryCache.Set(userContext.UserId, startStudyWithLabel);
        }
        else
        {
            var startStudyWithoutLabels = new LiveStudies
            {
                UserId = userContext.UserId,
                TimePlanned = timerInt,
                Labels = new List<LabelDto>(),
                StartTime = DateTime.UtcNow,
                LastRefresh = DateTime.UtcNow
            };

            _memoryCache.Set(userContext.UserId, startStudyWithoutLabels);
        }

        return Ok(new ErrorMessage("Session started", StatusCodes.Status200OK));
    }
    
    
    /// <summary>
    /// Permet d'arrêter une session de travail
    /// </summary>
    /// <returns></returns>
    [HttpPost("stop")]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErrorMessage))]
    public async Task<ActionResult> StopStudies()
    {
        var userContext = (User) HttpContext.Items["User"];

        // Si l'utilisateur n'est pas connecté,
        if (userContext == null)
        {
            return Forbid("Not access");
        }

        var liveStudy = (LiveStudies) _memoryCache.Get(userContext.UserId);
        if (liveStudy == null)
        {
            return NotFound(new ErrorMessage("This user has no session to stop.", StatusCodes.Status406NotAcceptable));
        }
        
        // On supprime l'ancienne session
        _memoryCache.Remove(userContext.UserId);

        if (liveStudy.LastRefresh.AddMinutes(MinutesMinimumHeatbeat) < DateTime.UtcNow)
        {
            // On la sauvegarde dans la base de données avec le temps du dernier battement
            var study = new Studies(Guid.NewGuid().ToString(),
                ((int) (liveStudy.LastRefresh - liveStudy.StartTime).TotalSeconds).ToString(CultureInfo.InvariantCulture),
                liveStudy.StartTime, GetPersonalGroup());
            
            study.Labels = new List<Label>();
            liveStudy.Labels.ForEach(label =>
            {
                var labelDb = _context.Labels.FirstOrDefault(l => l.LabelId == label.LabelId && l.UserId == userContext.UserId);
                if (labelDb != null)
                {
                    study.Labels.Add(labelDb);
                }
            });
            
            _context.Studies.Add(study);
            await _context.SaveChangesAsync();
        }
        else
        {
            // On la sauvegarde dans la base de données avec le temps actuel
            var study = new Studies(Guid.NewGuid().ToString(),
                ((int) (DateTime.UtcNow - liveStudy.LastRefresh).TotalSeconds).ToString(CultureInfo.InvariantCulture),
                liveStudy.StartTime, GetPersonalGroup());

            study.Labels = new List<Label>();
            liveStudy.Labels.ForEach(label =>
            {
                var labelDb = _context.Labels.FirstOrDefault(l => l.LabelId == label.LabelId && l.UserId == userContext.UserId);
                if (labelDb != null)
                {
                    study.Labels.Add(labelDb);
                }
            });
            
            _context.Studies.Add(study);
            await _context.SaveChangesAsync();
        }
        
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
        var userContext = (User) HttpContext.Items["User"];

        // Si l'utilisateur n'est pas connecté,
        if (userContext == null)
        {
            return Forbid("Not access");
        }

        var liveStudy = (LiveStudies) _memoryCache.Get(userContext.UserId);
        if (liveStudy == null)
        {
            return NotFound(new ErrorMessage("This user has no session to give an heatbeat.", StatusCodes.Status406NotAcceptable));
        }
        
        // S'il a été actualisé il y a plus de 2 {MinutesMinimumHeatbeat} minutes
        if (liveStudy.LastRefresh.AddMinutes(MinutesMinimumHeatbeat) < DateTime.UtcNow)
        {
            // On supprime l'ancienne session
            _memoryCache.Remove(userContext.UserId);
                
            // On la sauvegarde dans la base de données
            var study = new Studies(Guid.NewGuid().ToString(),
                ((int) (liveStudy.LastRefresh - liveStudy.StartTime).TotalSeconds).ToString(CultureInfo.InvariantCulture)
                , liveStudy.StartTime, GetPersonalGroup());
            
            study.Labels = new List<Label>();
            liveStudy.Labels.ForEach(label =>
            {
                var labelDb = _context.Labels.FirstOrDefault(l => l.LabelId == label.LabelId && l.UserId == userContext.UserId);
                if (labelDb != null)
                {
                    study.Labels.Add(labelDb);
                }
            });
            
            _context.Studies.Add(study);
            await _context.SaveChangesAsync();
                
            return NotFound(new ErrorMessage("The last session was unactive for more than " + MinutesMinimumHeatbeat + " minutes. The session has been saved.", StatusCodes.Status406NotAcceptable));
        }

        liveStudy.LastRefresh = DateTime.UtcNow;
        _memoryCache.Set(userContext.UserId, liveStudy);

        return Ok(new ErrorMessage("Heartbeat sent", StatusCodes.Status200OK));
    }

    /// <summary>
    /// return the studies with the studies number
    /// </summary>
    /// <param name="studiesId">the studies number</param>
    /// <returns></returns>
    [HttpGet("{studiesId}")]
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
                s.StudiesCreatedDate >= DateTime.Now.AddDays(-6))
            .ToList();

        Dictionary<string, float> data = new Dictionary<string, float>();
        studiesLastWeeks.ForEach((s) =>
        {
            bool isParsed = int.TryParse(s.StudiesTime, out int studiedTime);
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

    private int GetPersonalGroup()
    {
        var userConterxt = (User) HttpContext.Items["User"];

        var groups = _context.Groups.Where(g => g.GroupsIsPrivate && g.OwnerId == userConterxt.UserId)
            .Select(g => new GroupDto(g))
            .ToList();

        return groups.First().GroupId;
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