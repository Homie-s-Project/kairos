using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Kairos.API.Utils.OAuth2.Microsoft.Models;

namespace Kairos.API.Utils;

public class MicrosoftClient
{
    public const string OAuthUser = "https://graph.microsoft.com/v1.0/me";

    private string UserToken;
    
    public MicrosoftClient(string encryptedUserToken)
    {
        UserToken = CryptoUtils.Decrypt(encryptedUserToken);
    }

    public Task<UserMicrosoft> GetUserAsync()
    {
        return OAuthUser
            .WithOAuthBearerToken(UserToken)
            .GetJsonAsync<UserMicrosoft>();
    }
}