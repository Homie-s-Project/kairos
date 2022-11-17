using Newtonsoft.Json;

namespace Kairos.API.Utils.Google.Models;

public class Birthdays : Json
{
    [JsonProperty("date")] public DateGoogle Date { get; set; }
}

public class DateGoogle
{
    [JsonProperty("year")] public int Year { get; set; }
    [JsonProperty("month")] public int Month { get; set; }
    [JsonProperty("day")] public int Day { get; set; }
}