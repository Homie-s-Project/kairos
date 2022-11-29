using Newtonsoft.Json;

namespace Kairos.API.Utils.Google.Models;

public class EmailAddresses : Json
{
    [JsonProperty("value")] public string Value { get; set; }
}