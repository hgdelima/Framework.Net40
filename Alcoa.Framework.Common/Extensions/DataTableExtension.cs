using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps extend Datatable functions
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        /// Casts a datatable to IEnumerable of specific type
        /// </summary>
        public static IEnumerable<T> AsEnumerable<T>(this DataTable table) where T : new()
        {
            //Check for table availability
            if (table == null)
                throw new NullReferenceException("DataTable");

            //Get property length
            int propertiesLength = typeof(T).GetProperties().Length;

            //If no properties stop
            if (propertiesLength == 0)
                throw new NullReferenceException("Properties");

            //Create list to hold object T values
            var objList = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                //Create a new instance of our object T
                var obj = new T();

                //Get properties of object T
                PropertyInfo[] objProperties = obj.GetType().GetProperties();

                //iterate thru and populate property values

                for (int i = 0; i < propertiesLength; i++)
                {
                    //Get current property
                    PropertyInfo property = objProperties[i];

                    //Check datatable to see if datacolumn exists
                    if (table.Columns.Contains(property.Name))
                    {
                        //Get row cell value
                        object objValue = row[property.Name];

                        //Check for nullable property type and handle
                        var propertyType = property.PropertyType;

                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            propertyType = propertyType.GetGenericArguments()[0];

                        //Set property value
                        objProperties[i].SetValue(obj, Convert.ChangeType(objValue, propertyType, CultureInfo.CurrentCulture), null);
                    }
                }

                //Add to obj list
                objList.Add(obj);
            }

            return objList;
        }
    }
}