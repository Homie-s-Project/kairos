using System;
using System.Linq;
using System.Threading.Tasks;
using Kairos.API.Context;
using Kairos.API.Models;
using Kairos.API.Utils;
using Kairos.API.Utils.Google;
using Kairos.API.Utils.Microsoft;
using Kairos.API.Utils.Microsoft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kairos.API.Controllers;

[Route("auth")]
public class Authentification : BaseController
{
    private static readonly Random Random = new();
    private static IWebHostEnvironment _env;
    private readonly ILogger<Authentification> _logger;

    private readonly KairosContext _context;
    private readonly IMemoryCache _memoryCache;
    private readonly IConfiguration _config;

    private readonly string _microsoftClientId;
    private readonly string _microsoftClientSecret;
    private readonly string _microsoftTenantId;
    private readonly string _microsoftRedirectUri;
    private readonly string _microsoftRedirectFront;

    private readonly string _googleClientId;
    private readonly string _googleClientSecret;
    private readonly string _googleRedirectUri;
    private readonly string _googleRedirectFront;

    public Authentification(ILogger<Authentification> logger, KairosContext context, IMemoryCache memoryCache,
        IConfiguration config, IWebHostEnvironment env)
    {
        _logger = logger;

        _context = context;
        _memoryCache = memoryCache;
        _env = env;
        _config = config;

        // Récupération des paramètres de configuration pour Microsoft
        var configurationSectionMicrosoft = _config.GetSection("Authentication:Microsoft");
        _microsoftClientId = configurationSectionMicrosoft["ClientId"];
        _microsoftClientSecret = configurationSectionMicrosoft["ClientSecret"];
        _microsoftTenantId = configurationSectionMicrosoft["TenantId"];
        _microsoftRedirectUri = configurationSectionMicrosoft["RedirectUri"];
        _microsoftRedirectFront = configurationSectionMicrosoft["RedirectFront"];

        // Récupération des paramètres de configuration pour Google
        var configurationSectionGoogle = _config.GetSection("Authentication:Google");
        _googleClientId = configurationSectionGoogle["ClientId"];
        _googleClientSecret = configurationSectionGoogle["ClientSecret"];
        _googleRedirectUri = configurationSectionGoogle["RedirectUri"];
        _googleRedirectFront = configurationSectionGoogle["RedirectFront"];
    }

    /// <summary>
    /// Permet de se connecter à l'applicaation
    /// </summary>
    /// <returns>La redirection sur le système d'authentification de Microsoft</returns>
    [HttpGet("login/microsoft")]
    [AllowAnonymous]
    public async Task<RedirectResult> LoginMicrosoft()
    {
        // Génération d'un string unique qui sera utilisé pour la redirection et éviter les attaques CSRF
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string generateState = new string(Enumerable.Repeat(chars, 20)
            .Select(s => s[Random.Next(s.Length)]).ToArray());

        // On génère un lien de redirection vers le système d'authentification de Microsoft
        var opts = new OAuth2Microsoft.AuthorizeOptions
        {
            ClientId = _microsoftClientId,
            TenantId = _microsoftTenantId,
            RedirectUri = _microsoftRedirectUri,
            State = generateState,
            Scope =
                "https://graph.microsoft.com/User.Read.All https://graph.microsoft.com/User.Read https://graph.microsoft.com/profile https://graph.microsoft.com/email"
        };

        // Création d'un cache pour directement refusé les connexions qui durent plus de 5 minutes
        var cacheEntryOption = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        // On ajoute l'entrée dans le cache
        _memoryCache.Set(generateState, DateTime.Now, cacheEntryOption);

        // On redirige vers le système d'authentification de Microsoft
        return Redirect(OAuth2Microsoft.OAuthHelper.GetAuthorizeUrl(opts));
    }

    /// <summary>
    /// Permet de se connecter à l'applicaation
    /// </summary>
    /// <returns>La redirection sur le système d'authentification de Google (ou le service choisie)</returns>
    [HttpGet("login/google")]
    [AllowAnonymous]
    public async Task<RedirectResult> LoginGoogle()
    {
        // Génération d'un string unique
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string generateState = new string(Enumerable.Repeat(chars, 20)
            .Select(s => s[Random.Next(s.Length)]).ToArray());

        // On génère un lien de redirection vers le système d'authentification de Google
        var opts = new OAuth2Google.AuthorizeOptions
        {
            ClientId = _googleClientId,
            RedirectUri = _googleRedirectUri,
            State = generateState,
            Scope =
                "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/user.birthday.read"
        };

        // Création d'un cache pour directement refusé les connexions qui durent plus de 5 minutes
        var cacheEntryOption = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        // On ajoute l'entrée dans le cache
        _memoryCache.Set(generateState, DateTime.Now, cacheEntryOption);

        // On redirige vers le système d'authentification de Google
        return Redirect(OAuth2Google.OAuthHelper.GetAuthorizeUrl(opts));
    }

    /// <summary>
    /// Reçois la requête de microsoft pour la connexion
    /// </summary>
    /// <param name="code">Le code de microsoft concernant la connexion</param>
    /// <param name="state">Le code qu'on à générer qui permet de protéger contre les Man In The Middle</param>
    /// <param name="error">Le code erreur s'il y en a une</param>
    /// <param name="errorDescription">La description de l'erreur</param>
    /// <response code="200">Returns the token information</response>
    /// <response code="404">Can't found your account</response>
    /// <response code="401">You are not authorize</response>
    /// <response code="400">Microsoft can't find your account, please contact the owner of the application</response>
    /// <returns></returns>
    [HttpGet("callback/microsoft")]
    [AllowAnonymous]
    public async Task<IActionResult> CallBackMicrosoft(string code, string state, string error, string errorDescription)
    {
        // On vérifie que on reçois bien un code de retour de la connexion
        if (code == null)
        {
            // Si il n'y a aucun  code et qu'il y a une erreur
            if (error != null && errorDescription != null)
            {
                // Si l'enviroment est en développement on affiche l'erreur dans la console
                if (_env.IsDevelopment())
                {
                    _logger.LogWarning("Error when trying connecting user.\n\n" + error + ": " + errorDescription);
                }

                // On retourne une erreur 400 pour informer l'utilisateur que la connexion à échoué
                return BadRequest(new ErrorMessage("Error: '" + error + "', Please contact the admin.", StatusCodes.Status500InternalServerError));
            }

            // Si aucun code n'est retourné et qu'il n'y a pas d'erreur on retourne une erreur 404 pour informer l'utilisateur que aucun code n'a été retourné
            return NotFound(new ErrorMessage("No code parameter to connect to your account found.", StatusCodes.Status404NotFound));
        }

        // Si aucun "state" n'est retourné
        if (state == null)
        {
            // Si le state qu'on a générer et ajouté dans la mémoir cache n'est pas retourné par microsoft alors on retourne une erreur 404 pour informer l'utilisateur que la connexion à échoué
            return NotFound(new ErrorMessage("No state parameter to connect to your account found.", StatusCodes.Status404NotFound));
        }

        // On check si la connexion n'a demande de connexion n'a pas été faite il y a plus de 5 mintues grâce au state.
        if (!_memoryCache.TryGetValue(state, out DateTime outState))
        {
            // Si la connexion à été faite il y a plus de 5 minutes on retourne une erreur 401 pour informer l'utilisateur que la connexion à échoué
            return Unauthorized(new ErrorMessage("Your connection is too old, please try again.", StatusCodes.Status401Unauthorized));
        }

        OAuthResponse token = null;
        try
        {
            // On fait la demande du token à Microsoft
            token = await OAuth2Microsoft.OAuthHelper.GetAccessTokenAsync(code, _microsoftClientId,
                _microsoftClientSecret, _microsoftTenantId,
                _microsoftRedirectUri);
        }
        catch
        {
            // Si la demande échoue on retourne une erreur 401 pour informer l'utilisateur que la connexion à échoué
            return Unauthorized(new ErrorMessage("Can't get your token, please try again.", StatusCodes.Status401Unauthorized));
        }

        if (token == null)
        {
            // Si on ne reçoit aucun token on retourne une erreur 401 pour informer l'utilisateur que la connexion à échoué
            return BadRequest(new ErrorMessage("Can't get your token, please try again.", StatusCodes.Status401Unauthorized));
        }

        var accessToken = token.AccessToken;
        var encryptedAccessToken = CryptoUtils.Encrypt(accessToken);

        // On récupère les informations de l'utilisateur
        var client = await new MicrosoftClient(encryptedAccessToken).GetUserAsync();

        // On crée un utilisateur
        var newUser = new User(client.Id, client.GivenName, client.Surname, new DateTime(), client.Mail,
            DateTime.UtcNow);

        var tokenString = "";

        // On vérifie si l'utilisateur existe déjà
        var findUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
        if (findUser == null)
        {
            // S'il n'existe pas on l'ajoute dans la base de donnée
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var newOAuth2Credentials =
                new OAuth2Credentials(encryptedAccessToken, newUser.UserId);
            _context.OAuth2Credentials.Add(newOAuth2Credentials);
            await _context.SaveChangesAsync();

            var group = new Group(newUser.FirstName + " " + newUser.LastName + "'s group", newUser.UserId, true);
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            tokenString = JwtUtils.GenerateJsonWebToken(newUser);
        }
        else
        {
            // S'il existe on le met à jour
            findUser.FirstName = newUser.FirstName;
            findUser.LastName = newUser.LastName;
            findUser.LastUpdatedAt = DateTime.UtcNow;

            _context.Users.Update(findUser);
            await _context.SaveChangesAsync();

            var findOAuth2Credentials =
                await _context.OAuth2Credentials.FirstOrDefaultAsync(auth => auth.UserId == findUser.UserId);
            var findGroup =
                await _context.Groups.FirstOrDefaultAsync(u => u.GroupsIsPrivate && u.OwnerId == findUser.UserId);

            tokenString = JwtUtils.GenerateJsonWebToken(findUser);

            // Check le OAuth
            if (findOAuth2Credentials == null)
            {
                var newOAuth2Credentials =
                    new OAuth2Credentials(encryptedAccessToken, newUser.UserId);
                _context.OAuth2Credentials.Add(newOAuth2Credentials);
                await _context.SaveChangesAsync();
            }
            else
            {
                findOAuth2Credentials.AccessToken = encryptedAccessToken;

                _context.OAuth2Credentials.Update(findOAuth2Credentials);
                await _context.SaveChangesAsync();
            }

            // Check le group
            if (findGroup == null)
            {
                var group = new Group(newUser.FirstName + " " + newUser.LastName + "'s group", findUser.UserId, true);
                _context.Groups.Add(group);
                await _context.SaveChangesAsync();
            }
        }

        // Ajout du jwt dans les cookies de l'utilisateur
        Response.Cookies.Append("jwt", tokenString);

        // Si l'environement est sur développement on affiche le token dans la console, pour le développement
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("New JWT token generated: {TokenString}", tokenString);
        }

        return Redirect(_microsoftRedirectFront);
    }

    /// <summary>
    /// Reçois la requête de google pour la connexion
    /// </summary>
    /// <param name="code">Le code de microsoft concernant la connexion</param>
    /// <param name="state">Le code qu'on à générer qui permet de protéger contre les Man In The Middle</param>
    /// <param name="error">Le code erreur s'il y en a une</param>
    /// <param name="errorDescription">La description de l'erreur</param>
    /// <response code="200">Returns the token information</response>
    /// <response code="404">Can't found your account</response>
    /// <response code="401">You are not authorize</response>
    /// <response code="400">Google can't find your account, please contact the owner of the application</response>
    /// <returns></returns>
    [HttpGet("callback/google")]
    [AllowAnonymous]
    public async Task<IActionResult> CallBackGoogle(string code, string state, string error)
    {
        // Si aucun code
        if (code == null)
        {
            // Si il n'y a aucun  code et qu'il y a une erreur
            if (error != null)
            {
                if (_env.IsDevelopment())
                {
                    _logger.LogWarning("Error when trying connecting user.\n\n" + error);
                }

                // On retourne une erreur 500 si une erreur à été reçu
                return BadRequest(new ErrorMessage("Error: '" + error + "', Please contact the admin.", StatusCodes.Status500InternalServerError));
            }

            // On retourne une erreur 404 si aucun code n'a été reçu
            return NotFound(new ErrorMessage("No code parameter to connect to your account found.", StatusCodes.Status404NotFound));
        }

        // Si aucun "state" n'est retourné
        if (state == null)
        {
            // Si le state générer n'est pas renvoyé par google alors on retourne une erreur 404
            return NotFound(new ErrorMessage("No state parameter to connect to your account found.", StatusCodes.Status404NotFound));
        }

        // On check si la connexion n'a demande de connexion n'a pas été faite il y a plus de 5 mintues grâce au state.
        if (!_memoryCache.TryGetValue(state, out DateTime outState))
        {
            return Unauthorized(new ErrorMessage("Your connection is too old, please try again.", StatusCodes.Status401Unauthorized));
        }

        Utils.Google.Models.OAuthResponse token = null;
        try
        {
            // On fait la demande du token à Google
            token = await OAuth2Google.OAuthHelper.GetAccessTokenAsync(code, _googleClientId, _googleClientSecret,
                _googleRedirectUri);
        }
        catch
        {
            // Si il y a une erreur on retourne une erreur 500
            return Unauthorized(new ErrorMessage("Can't get your token, please try again.", StatusCodes.Status401Unauthorized));
        }

        if (token == null)
        {
            // Si aucune réponse n'est reçu on retourne une erreur 401
            return BadRequest(new ErrorMessage("Can't get your token, please try again.", StatusCodes.Status401Unauthorized));
        }

        var accessToken = token.AccessToken;
        var encryptedAccessToken = CryptoUtils.Encrypt(accessToken);

        // On fait la demande des informations de l'utilisateur à Google
        var client = await new GoogleClient(encryptedAccessToken).GetUserAsync();

        // On récupère les informations de l'utilisateur sur son compte google.
        var googleId = client.ResourceName.Replace("people/", "");
        var googleName = client.Names.First();
        var googleEmail = client.EmailAddresses.First();
        var birthdays = client.Birthdays.First();
        var googleBirthday = birthdays != null ? new DateTime(birthdays.Date.Year == 0 ? DateTime.UtcNow.Year : birthdays.Date.Year, birthdays.Date.Month, birthdays.Date.Day) : (DateTime?) null;
        
        // Si le compte google n'a qu'un pseudo
        if (googleName.FamilyName == null)
        {
            googleName.FamilyName = "-";
        }

        // On crée un utilisateur
        var newUser = new User(googleId, googleName.FamilyName, googleName.GivenName,
            googleBirthday, googleEmail.Value,
            DateTime.UtcNow);

        var tokenString = "";

        // On check si l'utilisateur existe déjà
        var findUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
        if (findUser == null)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var newOAuth2Credentials =
                new OAuth2Credentials(encryptedAccessToken, newUser.UserId);
            _context.OAuth2Credentials.Add(newOAuth2Credentials);
            await _context.SaveChangesAsync();

            var group = new Group(newUser.FirstName + " " + newUser.LastName + "'s group", newUser.UserId, true);
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            tokenString = JwtUtils.GenerateJsonWebToken(newUser);
        }
        else
        {
            // On update le user
            findUser.FirstName = newUser.FirstName;
            findUser.LastName = newUser.LastName;
            findUser.BirthDate = newUser.BirthDate;
            findUser.LastUpdatedAt = DateTime.UtcNow;

            _context.Users.Update(findUser);
            await _context.SaveChangesAsync();

            var findOAuth2Credentials =
                await _context.OAuth2Credentials.FirstOrDefaultAsync(auth => auth.UserId == findUser.UserId);
            var findGroup =
                await _context.Groups.FirstOrDefaultAsync(u => u.GroupsIsPrivate && u.OwnerId == findUser.UserId);

            tokenString = JwtUtils.GenerateJsonWebToken(findUser);

            // Check le OAuth
            if (findOAuth2Credentials == null)
            {
                var newOAuth2Credentials =
                    new OAuth2Credentials(encryptedAccessToken, newUser.UserId);
                _context.OAuth2Credentials.Add(newOAuth2Credentials);
                await _context.SaveChangesAsync();
            }
            else
            {
                findOAuth2Credentials.AccessToken = encryptedAccessToken;

                _context.OAuth2Credentials.Update(findOAuth2Credentials);
                await _context.SaveChangesAsync();
            }

            // Check le group
            if (findGroup == null)
            {
                var group = new Group(newUser.FirstName + " " + newUser.LastName + "'s group", findUser.UserId, true);
                _context.Groups.Add(group);
                await _context.SaveChangesAsync();
            }
        }

        // Ajout du jwt dans les cookies de l'utilisateur
        Response.Cookies.Append("jwt", tokenString);

        if (_env.IsDevelopment())
        {
            _logger.LogInformation("New JWT token generated: {TokenString}", tokenString);
        }

        // On redirige l'utilisateur vers la page d'accueil
        return Redirect(_googleRedirectFront);
    }
}