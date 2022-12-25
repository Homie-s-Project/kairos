using System.Linq;
using System.Threading.Tasks;
using Kairos.API.Context;
using Kairos.API.Utils;
using Microsoft.AspNetCore.Http;

namespace Kairos.API.Middleware;

// https://jasonwatmore.com/post/2021/06/02/net-5-create-and-validate-jwt-tokens-use-custom-jwt-middleware
public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, KairosContext kairosContext)
    {
        const string bearer = "Bearer";

        // On récupère le token dans le header de la requête
        string token = context.Request.Headers.Authorization;

        // Si le token n'est pas null et qu'il commence par "Bearer "
        if (token != null && token.Contains(bearer))
        {
            // On récupère le token sans le "Bearer "
            token = token.Remove(0, bearer.Length + 1);
        }

        // Récupère l'id de l'utilisateur
        var informationsJwt = JwtUtils.ValidateCurrentToken(token);

        // Si le token est valide
        if (informationsJwt != null)
        {
            // On récupère l'utilisateur dans le token.
            var userId = informationsJwt.Value;
            context.Items["User"] = kairosContext.Users.First(u => u.UserId == userId);
        }

        await _next(context);
    }
}