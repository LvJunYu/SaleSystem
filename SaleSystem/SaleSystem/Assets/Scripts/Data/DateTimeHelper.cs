﻿using System;

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

        public static string GetDateStr(this DateTime dateTime)
        {
            return string.Format("{0}.{1}.{2}", dateTime.Year, dateTime.Month, dateTime.Day);
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
            return checkOutDate1.GetDays() > checkInDate2.GetDays() && checkInData1.GetDays() < checkOutDate2.GetDays();
        }
    }
}