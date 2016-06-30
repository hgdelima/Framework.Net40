using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps manipulate IEnumerable operations
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Casts an IEnumerable to DataTable
        /// </summary>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            var oReturn = new DataTable(typeof(T).Name);
            object[] a_oValues;
            int i;

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo oProperty in properties)
                oReturn.Columns.Add(oProperty.Name, oProperty.PropertyType.ToBaseType());

            foreach (T item in list)
            {
                a_oValues = new object[properties.Length];

                for (i = 0; i < properties.Length; i++)
                    a_oValues[i] = properties[i].GetValue(item, null);

                oReturn.Rows.Add(a_oValues);
            }

            return oReturn;
        }
    }
}
