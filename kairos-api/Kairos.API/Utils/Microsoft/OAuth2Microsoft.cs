using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Kairos.API.Utils.Microsoft.Models;

namespace Kairos.API.Utils.Microsoft;

public class OAuth2Microsoft
{
    public class AuthorizeOptions
    {
        public string ClientId { get; set; }
        public string TenantId { get; set; }
        public string RedirectUri { get; set; }
        public string State { get; set; }
        public string Scope { get; set; }
    }

    public static class OAuthHelper
    {
        public const string OAuthEndpoint =
            "https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token";

        public const string AuthorizeEndpoint = "https://login.microsoftonline.com/{tenant}/oauth2/v2.0/authorize";

        /// <summary>
        /// On crée une url pour rediriger l'utilisateur vers la page de connexion de Microsoft
        /// </summary>
        /// <param name="opts">Les options de </param>
        public static string GetAuthorizeUrl(AuthorizeOptions opts)
        {
            var url = AuthorizeEndpoint.Replace("{tenant}", opts.TenantId)
                .SetQueryParam("client_id", opts.ClientId)
                .SetQueryParam("response_type", "code")
                .SetQueryParam("redirect_uri", opts.RedirectUri)
                .SetQueryParam("response_mode", "query")
                .SetQueryParam("state", opts.State)
                .SetQueryParam("scope", opts.Scope);

            return url;
        }

        /// <summary>
        /// On récupère le token d'accès à partir du code de connexion
        /// </summary>
        /// <param name="code">Code reçu après la connexion</param>
        /// <param name="clientId">Le clientId de notre application</param>
        /// <param name="clientSecret">Le clientSecret de notre application</param>
        /// <param name="tenantId">L'id de notre application micrcosoft</param>
        /// <param name="redirectUri">Où souhaite le rediriger</param>
        public static Task<OAuthResponse> GetAccessTokenAsync(string code, string clientId, string clientSecret,
            string tenantId,
            string redirectUri)
        {
            var form = new
            {
                client_id = clientId,
                client_secret = clientSecret,
                redirect_uri = redirectUri,
                grant_type = "authorization_code",
                code,
            };

            // On remplace le tenant dans l'url par l'id de notre application
            return OAuthEndpoint.Replace("{tenant}", tenantId)
                .PostUrlEncodedAsync(form)
                .ReceiveJson<OAuthResponse>();
        }
    }
}