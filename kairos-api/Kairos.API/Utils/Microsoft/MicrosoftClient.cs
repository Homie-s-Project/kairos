using System.Threading.Tasks;
using Flurl.Http;
using Kairos.API.Utils.Microsoft.Models;

namespace Kairos.API.Utils.Microsoft;

public class MicrosoftClient
{
    public const string OAuthUser = "https://graph.microsoft.com/v1.0/me";

    private readonly string _userToken;

    public MicrosoftClient(string encryptedUserToken)
    {
        _userToken = CryptoUtils.Decrypt(encryptedUserToken);
    }

    /// <summary>
    /// On fait une requête à l'API Microsoft pour récupérer les informations de l'utilisateur
    /// </summary>
    /// <returns>L'utilisateur Microsoft conneté</returns>
    public Task<UserMicrosoft> GetUserAsync()
    {
        return OAuthUser
            .WithOAuthBearerToken(_userToken)
            .GetJsonAsync<UserMicrosoft>();
    }
}