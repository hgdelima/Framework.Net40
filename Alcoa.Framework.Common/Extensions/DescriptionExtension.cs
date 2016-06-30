using System.ComponentModel;
using System.Reflection;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps manipulate Description Attributes Annotations
    /// </summary>
    public static class DescriptionExtension
    {
        /// <summary>
        /// Gets description value as string from Description Attribute annotations
        /// </summary>
        public static string GetDescription(this object value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }
}