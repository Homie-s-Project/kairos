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