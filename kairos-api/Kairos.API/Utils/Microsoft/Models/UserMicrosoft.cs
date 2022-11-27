using Newtonsoft.Json;

namespace Kairos.API.Utils.Microsoft.Models;

public class UserMicrosoft : Json
{
    [JsonProperty("displayName")] public string DisplayUsername { get; set; }

    [JsonProperty("givenName")] public string GivenName { get; set; }

    [JsonProperty("jobTitle")] public string JobTitle { get; set; }

    [JsonProperty("mail")] public string Mail { get; set; }

    [JsonProperty("mobilePhone")] public string MobilePhone { get; set; }

    [JsonProperty("preferredLanguage")] public string PreferedLanguage { get; set; }

    [JsonProperty("surname")] public string Surname { get; set; }

    [JsonProperty("id")] public string Id { get; set; }
}