using System.Threading.Tasks;
using Flurl.Http;
using Kairos.API.Utils.Google.Models;

namespace Kairos.API.Utils.Google;

public class GoogleClient
{
    public const string OAuthUser =
        "https://people.googleapis.com/v1/people/me?personFields=birthdays,emailAddresses,names";

    private readonly string _userToken;

    public GoogleClient(string encryptedUserToken)
    {
        _userToken = CryptoUtils.Decrypt(encryptedUserToken);
    }

    public Task<UserGoogle> GetUserAsync()
    {
        return OAuthUser
            .WithOAuthBearerToken(_userToken)
            .GetJsonAsync<UserGoogle>();
    }
}