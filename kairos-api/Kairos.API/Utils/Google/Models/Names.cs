using Newtonsoft.Json;

namespace Kairos.API.Utils.Google.Models;

public class Names : Json
{
    [JsonProperty("displayName")] public string DisplayName { get; set; }
    [JsonProperty("familyName")] public string FamilyName { get; set; }
    [JsonProperty("givenName")] public string GivenName { get; set; }
    [JsonProperty("displayNameLastFirst")] public string DisplayNameLastFirst { get; set; }
    [JsonProperty("unstructuredName")] public string UnstructuredName { get; set; }
}