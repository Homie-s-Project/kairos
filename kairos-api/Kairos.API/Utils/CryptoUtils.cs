using Crypto.AES;
using Microsoft.Extensions.Configuration;

namespace Kairos.API.Utils;

public class CryptoUtils
{
    public static readonly IConfiguration _config;

    static CryptoUtils()
    {
        _config = Startup.StaticConfig;
    }
    
    /// <summary>
    /// Chiffre la valeur en paramètre
    /// </summary>
    /// <param name="toBeEncrypted">la valeur à chiffrer</param>
    /// <returns>la valeur chiffré</returns>
    public static string Encrypt(string toBeEncrypted)
    {
        return AES.EncryptString(_config["Secure:SecretKey"], toBeEncrypted);
    }

    /// <summary>
    /// Dé-chiffrer la valeur en paramètre
    /// </summary>
    /// <param name="encrypted">la valeur dé-chiffrer</param>
    /// <returns>la valeur dé-chiffrer</returns>
    public static string Decrypt(string encrypted)
    {
        return AES.DecryptString(_config["Secure:SecretKey"], encrypted);
    }
}