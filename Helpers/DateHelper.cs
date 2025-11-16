namespace RIMS.Helpers
{
    public static class DateHelper
    {
        public static int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }

        public static string GetAgeGroup(int age)
        {
            return age switch
            {
                < 0 => "Invalid Age",
                >= 0 and < 6 => "0-5 (Child)",
                >= 6 and < 12 => "6-11 (School Age)",
                >= 12 and < 18 => "12-17 (Teenager)",
                >= 18 and < 60 => "18-59 (Adult)",
                >= 60 => "60+ (Senior Citizen)"
            };
        }

        public static string ToRelativeTime(DateTime date)
        {
            var span = DateTime.Now - date;

            if (span.TotalSeconds < 60)
                return "just now";
            if (span.TotalMinutes < 60)
                return $"{(int)span.TotalMinutes} minutes ago";
            if (span.TotalHours < 24)
                return $"{(int)span.TotalHours} hours ago";
            if (span.TotalDays < 30)
                return $"{(int)span.TotalDays} days ago";
            if (span.TotalDays < 365)
                return $"{(int)(span.TotalDays / 30)} months ago";

            return $"{(int)(span.TotalDays / 365)} years ago";
        }

        public static DateTime GetPhilippinesTime()
        {
            try
            {
                var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
            }
            catch (TimeZoneNotFoundException)
            {
                // Fallback to UTC+8 if Manila timezone is not found
                return DateTime.UtcNow.AddHours(8);
            }
        }

        public static bool IsWorkingDay(DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

        public static int GetWorkingDays(DateTime start, DateTime end)
        {
            if (start > end)
                return 0;

            int days = 0;
            for (var date = start; date <= end; date = date.AddDays(1))
            {
                if (IsWorkingDay(date))
                    days++;
            }
            return days;
        }

        public static string FormatDateForDisplay(DateTime? date)
        {
            return date?.ToString("MMMM dd, yyyy") ?? "Not specified";
        }

        public static string FormatDateTimeForDisplay(DateTime? date)
        {
            return date?.ToString("MMMM dd, yyyy hh:mm tt") ?? "Not specified";
        }

        public static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            return StartOfWeek(dt, startOfWeek).AddDays(6);
        }

        public static DateTime StartOfMonth(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime EndOfMonth(DateTime dt)
        {
            return StartOfMonth(dt).AddMonths(1).AddDays(-1);
        }
    }
}