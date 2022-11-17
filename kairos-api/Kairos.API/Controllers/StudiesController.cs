using System;
using System.Threading.Tasks;
using Kairos.API.Models;
using Kairos.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Kairos.API.Controllers;

[Route(("studies"))]
public class StudiesController : SecurityController
{
    private readonly IMemoryCache _memoryCache;

    public StudiesController(IMemoryCache memoryCache)
    {
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

        var encryptMicrosoftId = CryptoUtils.Encrypt(user.ServiceId);
        if (!_memoryCache.TryGetValue(encryptMicrosoftId, out DateTime outState))
        {
            RefreshMemory(encryptMicrosoftId);
            return Ok("Started studies");
        }

        RefreshMemory(encryptMicrosoftId);
        return Ok("Continue studies");
    }

    private void RefreshMemory(string encryptMicrosoftId)
    {
        // Entrée du cache qui est égale à 5min.
        var cacheEntryOption = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        _memoryCache.Set(encryptMicrosoftId, DateTime.Now, cacheEntryOption);
    }
}