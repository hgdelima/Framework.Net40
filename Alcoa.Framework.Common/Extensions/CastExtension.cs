using System;
using System.Text.RegularExpressions;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps casting operations
    /// </summary>
    public static class CastExtension
    {
        /// <summary>
        /// Cast objects to Integer
        /// </summary>
        public static int ToInt(this object obj)
        {
            int value;
            Int32.TryParse(obj.ToNumberAsString(), out value);

            return value;
        }

        /// <summary>
        /// Cast objects to Integer nullable
        /// </summary>
        public static int? ToIntNullable(this object obj)
        {
            return (obj != null) ? new int?(obj.ToInt()) : new int?();
        }

        /// <summary>
        /// Cast objects to Short
        /// </summary>
        public static short ToInt16(this object obj)
        {
            short value;
            Int16.TryParse(obj.ToNumberAsString(), out value);

            return value;
        }

        /// <summary>
        /// Cast objects to Short nullable
        /// </summary>
        public static short? ToInt16Nullable(this object obj)
        {
            return (obj != null) ? new short?(obj.ToInt16()) : new short?();
        }

        /// <summary>
        /// Cast objects to Long
        /// </summary>
        public static long ToLong(this object obj)
        {
            long value;
            Int64.TryParse(obj.ToNumberAsString(), out value);

            return value;
        }

        /// <summary>
        /// Cast objects to Long nullable
        /// </summary>
        public static long? ToLongNullable(this object obj)
        {
            return (obj != null) ? new long?(obj.ToLong()) : new long?();
        }

        /// <summary>
        /// Cast objects to Decimal
        /// </summary>
        public static decimal ToDecimal(this object obj)
        {
            decimal value;
            decimal.TryParse(obj.ToNumberAsString(), out value);

            return value;
        }

        /// <summary>
        /// Cast objects to Long nullable
        /// </summary>
        public static decimal? ToDecimalNullable(this object obj)
        {
            return (obj != null) ? new decimal?(obj.ToDecimal()) : new decimal?();
        }

        /// <summary>
        /// Cast objects to Boolean
        /// </summary>
        public static bool ToBool(this object obj)
        {
            var value = default(bool);
            
            if (obj != null)
                bool.TryParse(obj.ToString(), out value);

            return value;
        }

        /// <summary>
        /// Cast objects to Boolean nullable
        /// </summary>
        public static bool? ToBoolNullable(this object obj)
        {
            return (obj != null) ? new bool?(obj.ToBool()) : new bool?();
        }

        /// <summary>
        /// Cast objects to DateTime
        /// </summary>
        public static DateTime ToDateTime(this object obj)
        {
            var value = default(DateTime);

            if (obj != null)
                DateTime.TryParse(obj.ToString(), out value);

            return value;
        }

        /// <summary>
        /// Cast objects to DateTime nullable
        /// </summary>
        public static DateTime? ToDateTimeNullable(this object obj)
        {
            return (obj != null) ? new DateTime?(obj.ToDateTime()) : new DateTime?();
        }

        /// <summary>
        /// Cast objects to Numeric value as string type
        /// </summary>
        public static string ToNumberAsString(this object obj)
        {
            return (obj == null) ?
                string.Empty :
                new Regex(@"\D+").Replace(obj.ToString(), string.Empty);
        }

        /// <summary>
        /// Checks if an object is Numeric value
        /// </summary>
        public static bool IsNumber(this object obj)
        {
            return (obj == null) ?
                false :
                new Regex(@"\D+", RegexOptions.Compiled).Match(obj.ToString()).Success;
        }

        /// <summary>
        /// Cast objects to Char value as string type
        /// </summary>
        public static char ToChar(this object obj)
        {
            var value = default(char);

            if (obj != null)
                char.TryParse(obj.ToString(), out value);

            return value;
        }

        /// <summary>
        /// Cast types to Underlying Types
        /// </summary>
        public static Type ToBaseType(this Type type)
        {
            if (type != null &&
                type.IsValueType &&
                type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return Nullable.GetUnderlyingType(type);
            else
                return type;
        }
    }
}