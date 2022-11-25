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
    /// <response code="200">Return user info</response>
    /// <response code="403">If no user is not connected</response>   
    /// <returns>Retourne l'utilisateur connecté</returns>
    [HttpGet("me", Name = "Get Current User Info")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetMe()
    {
        var userContext = (User) HttpContext.Items["User"];

        // Si l'utilisateur est trouvé grâce au cookie
        if (userContext != null)
        {
            return Ok(new UserDto(userContext));
        }

        return Forbid("not access");
    }
}