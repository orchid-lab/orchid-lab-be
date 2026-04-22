namespace orchid_backend_net.Application.Common.Helper
{
    public static class TimeZoneHelper
    {
        private static readonly TimeZoneInfo VietnamTimeZoneInfo =
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        /// <summary>
        /// only for using in business check, not using for storing in database
        /// </summary>
        public static DateTime VietnamTimeNow
        {
            get
            {
                var now = TimeZoneInfo.ConvertTimeFromUtc(
                    DateTime.UtcNow, VietnamTimeZoneInfo);

                return new DateTime(
                    now.Year,
                    now.Month,
                    now.Day,
                    now.Hour,
                    now.Minute,
                    0,
                    DateTimeKind.Unspecified);
            }
        }

        /// <summary>
        /// Check if time is in working hour (7h - 17h)
        /// Seconds are ignored
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsInWorkingHour(DateTime dateTime)
        {
            TimeSpan start = new(7, 0, 0);
            TimeSpan end = new(17, 0, 0);
            return dateTime.TimeOfDay >= start && dateTime.TimeOfDay < end;
        }

        public static DateTime ToVietnamTime(this DateTime utcDateTime)
        => TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc),
            VietnamTimeZoneInfo);

        public static string ToVietnamTimeString(this DateTime utcDateTime, string format = "dd/MM/yyyy HH:mm")
            => utcDateTime.ToVietnamTime().ToString(format);
    }

}
