using System;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps manipulate DateTime formats
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// Checks if a date is between a start date and an end date time
        /// </summary>
        public static bool Between(this DateTime dateTime, DateTime startDateTime, DateTime endDateTime)
        {
            return Between(dateTime, startDateTime, endDateTime, true);
        }

        /// <summary>
        /// Checks if a date is between a start date and an end date, and can ignore time on checking
        /// </summary>
        public static bool Between(this DateTime date, DateTime startDateTime, DateTime endDateTime, bool ignoreTime)
        {
            if (ignoreTime)
                return ((endDateTime.Date >= date.Date) && (startDateTime.Date <= date.Date));

            return ((endDateTime >= date) && (startDateTime <= date));
        }
    }
}