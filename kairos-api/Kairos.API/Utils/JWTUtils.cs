using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Crypto.AES;
using Kairos.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Kairos.API.Utils;

public static class JwtUtils
{
    public static readonly IConfiguration _config;

    static JwtUtils()
    {
        _config = Startup.StaticConfig;
    }

    /// <summary>
    /// Chiffre le texte en paramètre
    /// </summary>
    /// <param name="toBeEncrypted">texte à faire chiffrer</param>
    /// <returns>le texte chiffré</returns>
    private static string Encrypt(string toBeEncrypted)
    {
        var secretKey = _config.GetSection("Jwt")["SecretKey"];
        return AES.EncryptString(secretKey, toBeEncrypted);
    }

    /// <summary>
    /// Dé-chiffre le texte en pramètre
    /// </summary>
    /// <param name="encrypted">texte à faire dé-chiffrer</param>
    /// <returns>le texte déchiffré</returns>
    private static string Decrypt(string encrypted)
    {
        var secretKey = _config.GetSection("Jwt")["SecretKey"];
        return AES.DecryptString(secretKey, encrypted);
    }

    /// <summary>
    /// Genère le JWT
    /// </summary>
    /// <param name="user">l'utilisateur concerné</param>
    /// <param name="logUser">les informations de connexion</param>
    /// <returns>le jwt chiffré</returns>
    public static string GenerateJsonWebToken(User user)
    {
        // Récupèration de la clé de chiffrement
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("Jwt")["Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        // Création des claims
        var claims = new[]
        {
            new Claim("id", user.UserId.ToString()),
            new Claim("microsoftId", user.ServiceId),
            new Claim("DateOfJoining", user.CreatedAt.ToString(CultureInfo.InvariantCulture)),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Récupèration de la configuration du token
        var configurationSection = _config.GetSection("Jwt");
        var jwtIssuer = configurationSection["Issuer"];
        var jwtAudience = configurationSection["Audience"];

        // Génération du token JWT
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(120),
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            SigningCredentials = credentials
        };

        // Création du token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        /* Encrytion du token
         *
         * Cette partie n'est pas obligatoire, mais permet de rendre le token plus difficile à lire par un utilisateur lambda ou un attaquant
         */
        var beEncrypted = tokenHandler.WriteToken(token);
        return Encrypt(beEncrypted);
    }

    /// <summary>
    /// Permet de validé le jwt donné
    /// </summary>
    /// <param name="token">le token</param>
    /// <returns>le user id et son log id, qui permet d'avoir l'id de l'utilisateur et l'id de sa connexion</returns>
    public static int? ValidateCurrentToken(string token)
    {
        // On vérifie directement si le token n'est pas vide
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        // On récupère la clé de chiffrement
        var jwtKey = _config.GetSection("Jwt")["Key"];

        // On génère le token handler et la clé de chiffrement
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
        var tokenHandler = new JwtSecurityTokenHandler();

        // On essaye de déchiffrer le token
        try
        {
            var configurationSection = _config.GetSection("Jwt");
            var validIssuer = configurationSection["Issuer"];
            var validAudience = configurationSection["Audience"];

            // On decrypte le token suite à l'encryption
            var decrpytedToken = Decrypt(token);

            // On vérifie que le token est valide
            tokenHandler.ValidateToken(decrpytedToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = securityKey,
            }, out SecurityToken validatedToken);

            // Si le token est valide on récupère l'id de l'utilisateur
            var jwtToken = (JwtSecurityToken) validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            return userId;
        }
        catch
        {
            return null;
        }
    }
}