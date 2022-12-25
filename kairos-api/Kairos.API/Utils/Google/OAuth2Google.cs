using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Kairos.API.Utils.Google.Models;

namespace Kairos.API.Utils.Google;

public class OAuth2Google
{
    public class AuthorizeOptions
    {
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string State { get; set; }
        public string Scope { get; set; }
    }

    public static class OAuthHelper
    {
        public const string OAuthEndpoint =
            "https://oauth2.googleapis.com/token";

        public const string AuthorizeEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";

        /// <summary>
        /// On crée une url pour rediriger l'utilisateur vers la page de connexion de Google
        /// </summary>
        /// <param name="opts">Les options de connexion</param>
        /// <returns>l'url</returns>
        public static string GetAuthorizeUrl(AuthorizeOptions opts)
        {
            var url = AuthorizeEndpoint
                .SetQueryParam("client_id", opts.ClientId)
                .SetQueryParam("response_type", "code")
                .SetQueryParam("redirect_uri", opts.RedirectUri)
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
        /// <param name="redirectUri">Où souhaite le rediriger</param>
        public static Task<OAuthResponse> GetAccessTokenAsync(string code, string clientId, string clientSecret,
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

            return OAuthEndpoint
                .PostUrlEncodedAsync(form)
                .ReceiveJson<OAuthResponse>();
        }
    }
}