using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Kairos.API.Controllers;

[ApiController]
[Route("[controller]/auth")]
public class Authentification : ControllerBase
{
    private static readonly Random Random = new();

    private readonly IMemoryCache _memoryCache;

    private readonly ILogger<Authentification> _logger;

    public Authentification(IMemoryCache memoryCache, ILogger<Authentification> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    /// <summary>
    /// Permet de se connecter à l'applicaation
    /// </summary>
    /// <returns>La redirection sur le système d'authentification de Google (ou le service choisie)</returns>
    [HttpGet("/login")]
    public RedirectResult Login()
    {
        // Génération d'un string unique
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string generateState = new string(Enumerable.Repeat(chars, 20)
            .Select(s => s[Random.Next(s.Length)]).ToArray());


        // Création d'un cache pour directement refusé les connexions qui durent plus de 5 minutes
        var cacheEntryOption = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        _memoryCache.Set(generateState, DateTime.Now, cacheEntryOption);

        _logger.LogInformation("Generated salt: {generateState}");

        return Redirect("http://localhost:5000/auth/login");
    }

    /// <summary>
    /// Retourne les informations de l'utilisateur s'il est connecté
    /// </summary>
    /// <returns>Retourne l'utilisateur connecté</returns>
    [HttpGet("/me")]
    public Task<IActionResult> Me()
    {
        
        return null;
    }
}