using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Kairos.API.Context;
using Kairos.API.Models;
using Kairos.API.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Kairos.API.Controllers;

[Route(("studies"))]
public class StudiesController : SecurityController
{
    private readonly IMemoryCache _memoryCache;
    private readonly KairosContext _context;

    public StudiesController(IMemoryCache memoryCache, KairosContext context)
    {
        _memoryCache = memoryCache;
        _context = context;
    }

    /// <summary>
    /// send heatbeat to check if user is still studying
    /// </summary>
    /// <returns></returns>
    [HttpPost("heartbeat")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> HeartBeat()
    {
        var user = (User) HttpContext.Items["User"];

        // Si l'utilisateur n'est pas connecté,
        if (user == null)
        {
            return Forbid("Not access");
        }

        var encryptMicrosoftId = CryptoUtils.Encrypt(user.ServiceId);
        if (!_memoryCache.TryGetValue(encryptMicrosoftId, out DateTime outState))
        {
            RefreshMemory(encryptMicrosoftId);
            return Ok("Started studies");
        }

        RefreshMemory(encryptMicrosoftId);
        return Ok("Continue studies");
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
            .FirstOrDefault(s => s.StudiesId == studiesIdParsed &&
                                 (s.Group.Users.FirstOrDefault(u => u.UserId == userConterxt.UserId) != null || s.Group.OwnerId == userConterxt.UserId));

        if (studies == null)
        {
            return NotFound(new ErrorMessage("Studies not found", StatusCodes.Status404NotFound));
        }

        return Ok(new StudiesDto(studies, false));
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
                s.Group.Users.FirstOrDefault(u => u.UserId == userConterxt.UserId) != null || 
                s.Group.OwnerId == userConterxt.UserId &&
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
            .Include(s => s.Labels)
            .Where(s => 
                s.Group.Users.FirstOrDefault(u => u.UserId == userConterxt.UserId) != null || 
                s.Group.OwnerId == userConterxt.UserId &&
                s.StudiesCreatedDate >= DateTime.Now.AddDays(-7))
            .Select(s => new StudiesDto(s, true))
            .ToList();


        // TODO: Les labels ne sont pas prit en compte
        Dictionary<string, float> data = new Dictionary<string, float>();
        studiesLastWeeks.ForEach((s) => {
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