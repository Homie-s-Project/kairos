using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kairos.API.Utils.Google.Models;

public class UserGoogle : Json
{
    [JsonProperty("names")] public List<Names> Names { get; set; }
    [JsonProperty("birthdays")] public List<Birthdays> Birthdays { get; set; }
    [JsonProperty("emailAddresses")] public List<EmailAddresses> EmailAddresses { get; set; }
    [JsonProperty("resourceName")] public string ResourceName { get; set; }
}