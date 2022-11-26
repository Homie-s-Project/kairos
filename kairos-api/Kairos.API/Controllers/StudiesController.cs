using System;
using System.Collections.Generic;
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

        // TODO: Check qu'il l'utilisateur est bien dans le groupe de l'étude
        var studies = _context.Studies
            .FirstOrDefault(s => s.StudiesId == studiesIdParsed);

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
        // TODO: données fictive
        Dictionary<string, int> data = new Dictionary<string, int>
        {
            {"Lundi", GenerateNumberBetween(1, 6)},
            {"Mardi", GenerateNumberBetween(1, 6)},
            {"Mercredi", GenerateNumberBetween(1, 6)},
            {"Jeudi", GenerateNumberBetween(1, 6)},
            {"Vendredi", GenerateNumberBetween(1, 6)},
            {"Samedi", GenerateNumberBetween(1, 6)},
            {"Dimanche", GenerateNumberBetween(1, 6)}
        };

        return Ok(new LastWeekHoursData(data));
    }

    /// <summary>
    /// return the last week hours of studies per label
    /// </summary>
    [HttpGet("lastWeek/hoursPerLabel")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LastWeekWorkPerLabel))]
    public IActionResult LastWeekHoursPerLabel()
    {
        // TODO: données fictive
        Dictionary<string, int> data = new Dictionary<string, int>
        {
            {"Science / Math", GenerateNumberBetween(1, 6)},
            {"Economie", GenerateNumberBetween(1, 6)},
            {"Allemand", GenerateNumberBetween(1, 6)},
            {"Anglais", GenerateNumberBetween(1, 6)},
            {"Informatique", GenerateNumberBetween(1, 6)}
        };

        return Ok(new LastWeekWorkPerLabel(data));
    }

    /// <summary>
    /// return the rate of studying last week
    /// </summary>
    [HttpGet("lastWeek/rate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public IActionResult LastWeekRate()
    {
        // TODO: Valeur factive
        var hours = GenerateNumberBetween(-50, 50);

        return Ok(hours);
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
    public Dictionary<string, int>.KeyCollection Labels { get; set; }
    public Dictionary<string, int>.ValueCollection Hours { get; set; }

    public LastWeekWorkPerLabel(Dictionary<string, int> data)
    {
        Labels = data.Keys;
        Hours = data.Values;
    }
}

public class LastWeekHoursData
{
    public Dictionary<string, int>.KeyCollection DayOfWeek { get; set; }
    public Dictionary<string, int>.ValueCollection Hours { get; set; }

    public LastWeekHoursData(Dictionary<string, int> data)
    {
        DayOfWeek = data.Keys;
        Hours = data.Values;
    }
}