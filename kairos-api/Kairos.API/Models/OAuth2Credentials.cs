using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class OAuth2Credentials
{
    public OAuth2Credentials(string accessToken, int userId)
    {
        AccessToken = accessToken;
        UserId = userId;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string AccessToken { get; set; }

    [ForeignKey("User")] public int UserId { get; set; }
    public virtual User User { get; set; }
}