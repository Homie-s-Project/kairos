using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kairos.API.Context;
using Kairos.API.Models;
using Kairos.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Kairos.API.Controllers;

[Route(("studies"))]
public class StudiesController : BaseController
{
    private readonly IMemoryCache _memoryCache;
    private readonly KairosContext _context;

    public StudiesController(IMemoryCache memoryCache, KairosContext context)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    [HttpGet("heartbeat")]
    public async Task<IActionResult> HeartBeat()
    {
        var user = (User) HttpContext.Items["User"];

        // Si l'utilisateur n'est pas connecté,
        if (user == null)
        {
            return Forbid("Not access");
        }

        var encryptMicrosoftId = CryptoUtils.Encrypt(user.MicrosoftId);
        if (!_memoryCache.TryGetValue(encryptMicrosoftId, out DateTime outState))
        {
            RefreshMemory(encryptMicrosoftId);
            return Ok("Started studies");
        }

        RefreshMemory(encryptMicrosoftId);
        return Ok("Continue studies");
    }

    [HttpGet("weeks")]
    public async Task<IActionResult> Weeks(DateTime startTime, DateTime endTime)
    {
        int totalTime = (endTime - startTime).Days;
        if (totalTime > 7)
        {
            BadRequest("You can't see more than 7 days.");
        }

        if (totalTime <= 0)
        {
            BadRequest("You can only see 7 days.");
        }

        List<Studies> studiesList = new List<Studies>(_context.Studies
            .Where(s => s.StudiesDate >= startTime && endTime <= s.StudiesDate));

        if (studiesList.Count > 0)
        {
            return Ok(studiesList);
        }

        return NotFound("No studies found between these dates.");
    }

    private void RefreshMemory(string encryptMicrosoftId)
    {
        // Entrée du cache qui est égale à 5min.
        var cacheEntryOption = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        _memoryCache.Set(encryptMicrosoftId, DateTime.Now, cacheEntryOption);
    }
}