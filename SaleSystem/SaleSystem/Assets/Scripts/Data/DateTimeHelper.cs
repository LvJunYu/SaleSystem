using System;

namespace Sale
{
    public static class DateTimeHelper
    {
        public static int GetDays(this DateTime dateTime)
        {
            return (dateTime - DateTime.MinValue).Days;
        }

        public static bool CheckDayValid(int day)
        {
            var res = day >= 1 && day <= 31;
            if (!res)
            {
                SocialGUIManager.ShowPopupDialog("请输入正确的时间");
            }

            return res;
        }

        public static bool CheckMonthValid(int month)
        {
            var res = month >= 1 && month <= 12;
            if (!res)
            {
                SocialGUIManager.ShowPopupDialog("请输入正确的时间");
            }

            return res;
        }

        public static bool CheckYearValid(int year)
        {
            var res = year > DateTime.MinValue.Year && year < 3000;
            if (!res)
            {
                SocialGUIManager.ShowPopupDialog("请输入正确的时间");
            }

            return res;
        }
    }
}