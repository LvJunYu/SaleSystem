using System;

namespace Sale
{
    public static class DateTimeHelper
    {
        public static int GetDays(this DateTime dateTime)
        {
            return (dateTime - DateTime.MinValue).Days;
        }

        public static DateTime GetDateTime(int days)
        {
            return DateTime.MinValue.AddDays(days);
        }

        public static int GetMonths(this DateTime dateTime)
        {
            return (dateTime.Year - DateTime.MinValue.Year) * 12 + dateTime.Month - 1;
        }

        public static DateTime GetDateTimeByMonth(int months)
        {
            return DateTime.MinValue.AddMonths(months);
        }

        public static string GetDateRaw(this DateTime dateTime)
        {
            return string.Format("{0}.{1}.{2}", dateTime.Year, dateTime.Month, dateTime.Day);
        }

        public static string GetDateStr(this DateTime dateTime)
        {
            return string.Format("{0}.{1}.{2}\n{3}:{4}", dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
                dateTime.Minute);
        }
    }
}