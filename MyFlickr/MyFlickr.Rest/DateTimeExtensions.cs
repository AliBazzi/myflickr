namespace System
{
    /// <summary>
    /// Extension Methods for DateTime.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Return Unix Timestamp from DateTime object.
        /// </summary>
        /// <param name="date">instance.</param>
        /// <returns>Double that represents Unix Timestamp.</returns>
        public static double ToUnixTimeStamp(this DateTime date)
        {
            return (date - new DateTime(1970, 1, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        }

        /// <summary>
        /// Return DateTime object from Unix Timestamp.
        /// </summary>
        /// <param name="unixTimestamp">instance.</param>
        /// <returns>DateTime object.</returns>
        public static DateTime ToDateTimeFromUnix(this double unixTimestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTimestamp);
        }
    }
}
