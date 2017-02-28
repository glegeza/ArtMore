namespace ArtMoreWPF
{
    using System;

    public static class TimeStringHelper
    {
        public static string GetHourBasedString(double seconds)
        {
            var span = TimeSpan.FromSeconds(seconds);
            var negativeString = seconds < 0 ? "-" : "";
            return String.Format("{0}{1:00}:{2:00}:{3:00}",
                negativeString,
                Math.Abs((int)span.TotalHours),
                Math.Abs(span.Minutes),
                Math.Abs(span.Seconds));
        }
    }
}
