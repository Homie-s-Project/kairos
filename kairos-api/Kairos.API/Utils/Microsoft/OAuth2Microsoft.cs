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

            return OAuthEndpoint.Replace("{tenant}", tenantId)
                .PostUrlEncodedAsync(form)
                .ReceiveJson<OAuthResponse>();
        }
    }
}