using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Core.Helper_
{
    public static class DateTimeExtension
    {
        public static DateTime ConvertTimeToUtc(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dateTime);
        }

        public static DateTime ConvertTimeFromUtc(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local);
        }
    }
}
