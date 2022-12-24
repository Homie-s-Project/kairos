using Kairos.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kairos.API.Controllers;

[Microsoft.AspNetCore.Components.Route("user")]
public class UserController : SecurityController
{
    /// <summary>
    /// Retourne les informations de l'utilisateur s'il est connecté
    /// </summary>
    /// <response code="200">Retourne les informations de l'utilisateur</response>
    /// <response code="403">Si aucun utilisateur n'est connecté on retourne une erreur</response>   
    /// <returns>Retourne l'utilisateur connecté</returns>
    [HttpGet("me", Name = "Récupère le status de connexion de l'utilisateur")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetMe()
    {
        var userContext = (User) HttpContext.Items["User"];

        // Si l'utilisateur est trouvé grâce au cookie
        if (userContext != null)
        {
            return Ok(new UserDto(userContext));
        }

        return Unauthorized(new ErrorMessage("No user connected", StatusCodes.Status401Unauthorized));
    }
}