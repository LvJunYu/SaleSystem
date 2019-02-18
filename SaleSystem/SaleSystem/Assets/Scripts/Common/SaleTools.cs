using System;

namespace Sale
{
    public static class SaleTools
    {
        public static int SafeIntParse(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return 0;
            }

            int num;
            int.TryParse(content, out num);
            return num;
        }

        public static long SafeLongParse(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return 0;
            }

            long num;
            long.TryParse(content, out num);
            return num;
        }

        public static bool CheckMinuteValid(int hour)
        {
            var res = hour >= 0 && hour <= 60;
            if (!res)
            {
                SocialGUIManager.ShowPopupDialog("日期输入有误");
            }

            return res;
        }

        public static bool CheckHourValid(int hour)
        {
            var res = hour >= 0 && hour <= 24;
            if (!res)
            {
                SocialGUIManager.ShowPopupDialog("日期输入有误");
            }

            return res;
        }

        public static bool CheckDayValid(int day)
        {
            var res = day >= 1 && day <= 31;
            if (!res)
            {
                SocialGUIManager.ShowPopupDialog("日期输入有误");
            }

            return res;
        }

        public static bool CheckMonthValid(int month)
        {
            var res = month >= 1 && month <= 12;
            if (!res)
            {
                SocialGUIManager.ShowPopupDialog("月份输入有误");
            }

            return res;
        }

        public static bool CheckYearValid(int year)
        {
            var res = year > DateTime.MinValue.Year && year < 3000;
            if (!res)
            {
                SocialGUIManager.ShowPopupDialog("年份超出范围");
            }

            return res;
        }

        public static bool IsConflict(DateTime checkInData1, DateTime checkOutDate1, DateTime checkInDate2,
            DateTime checkOutDate2)
        {
            return checkOutDate1 > checkInDate2 && checkInData1 < checkOutDate2;
        }
    }
}