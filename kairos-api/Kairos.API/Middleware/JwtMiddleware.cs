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
        const string BEARER = "Bearer";

        string token = context.Request.Headers.Authorization;

        if (token != null && token.Contains(BEARER))
        {
            token = token.Remove(0, BEARER.Length + 1);
        }

        // Récupère l'id de l'utilisateur
        var informationsJwt = JwtUtils.ValidateCurrentToken(token);

        if (informationsJwt != null)
        {
            var userId = informationsJwt.Value;
            context.Items["User"] = kairosContext.Users.First(u => u.UserId == userId);
        }

        await _next(context);
    }
}