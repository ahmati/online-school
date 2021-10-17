using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Core.Helper_
{
    public static class TimeSpanExtension
    {
        public static TimeSpan ConvertTimeToUtc(this TimeSpan timeSpan)
        {
            DateTime dt = new DateTime().Add(timeSpan);
            dt = dt.ConvertTimeToUtc();
            return dt.TimeOfDay;
        }

        public static TimeSpan ConvertTimeFromUtc(this TimeSpan timeSpan)
        {
            DateTime dt = new DateTime().Add(timeSpan);
            dt = dt.ConvertTimeFromUtc();
            return dt.TimeOfDay;
        }
    }
}
