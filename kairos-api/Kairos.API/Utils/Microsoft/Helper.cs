using System;

namespace Kairos.API.Utils.Microsoft;

public class Helper
{
    public static class TimeHelper
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetCurrentUnixTimestampSeconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return (long) (DateTime.UtcNow - UnixEpoch).TotalSeconds;
        }

        public static DateTimeOffset FromUnixTimestampSeconds(long seconds)
        {
            return DateTimeOffset.FromUnixTimeSeconds(seconds);
            return UnixEpoch.AddSeconds(seconds);
        }
    }
}