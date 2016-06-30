using System;
using System.Text.RegularExpressions;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps manipulate String operations
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Trim string to a specific max length
        /// </summary>
        public static string TrimLength(this string text, int length)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= length)
                return text;

            return text.Substring(0, length);
        }

        /// <summary>
        /// Split a string value using a string instead Char
        /// </summary>
        public static string[] Split(this string text, string splitter)
        {
            return text.Split(new[] { splitter }, StringSplitOptions.None);
        }

        /// <summary>
        /// Remove accents from a string value
        /// </summary>
        public static string RemoveAccents(this string text)
        {
            var withAccents = @"ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            var woutAccents = @"AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (var i = 0; i < withAccents.Length; i++)
                text = text.Replace(withAccents[i].ToString(), woutAccents[i].ToString()).Trim();

            return text;
        }

        /// <summary>
        /// Remove special chars from a string value
        /// </summary>
        public static string RemoveSpecialChars(this string text)
        {
            return Regex.Replace(text, @"[^a-zA-Z0-9\s]", string.Empty, RegexOptions.Compiled);
        }
    }
}