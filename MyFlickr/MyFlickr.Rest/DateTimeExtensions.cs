using System;

namespace MyFlickr.Rest
{
    public static class DateTimeExtensions
    {
        public static double ToUnixTimeStamp(this DateTime date)
        {
            return (date - new DateTime(1970, 1, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        }
    }
}
