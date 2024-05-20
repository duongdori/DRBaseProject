using UnityEngine;

namespace DR.Utilities
{
    public static class TimeHelpers
    {
        
        public static string SecondsToTimeString(double seconds)
        {
            var timeSpan = System.TimeSpan.FromSeconds(seconds);
            var totalHours = timeSpan.TotalHours;
            var remainingHours = (int)totalHours % 24;
            return string.Format("{0:00}:{1:00}:{2:00}", remainingHours, timeSpan.Minutes, timeSpan.Seconds);
        }

        public static System.DateTime GetCurrentTime(bool utc = false) => utc ? System.DateTime.UtcNow : System.DateTime.Now;

        public static System.TimeSpan GetCurrentTimeSpan(bool utc = false) => GetCurrentTime(utc).Subtract(System.DateTime.MinValue);

        public static System.DateTime FromEpochTime(long sec)
        {
            return new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(sec);
        }

        public static long ToEpochTime(System.DateTime time)
        {
            return (long)(time - new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }
    }
}
