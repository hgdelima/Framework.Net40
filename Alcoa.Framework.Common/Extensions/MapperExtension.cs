using EmitMapper;
using EmitMapper.MappingConfiguration;
using System;
using System.Linq;
using System.Reflection;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps manipulate Object mappings
    /// </summary>
    public static class MapperExtension
    {
        /// <summary>
        /// Clone object matching its property names
        /// </summary>
        public static T Clone<T>(this T obj)
        {
            return ObjectMapperManager.DefaultInstance
                .GetMapper<T, T>(new DefaultMapConfig()
                .MatchMembers((m1, m2) => m1.ToUpper() == m2.ToUpper()))
                .Map(obj);
        }

        /// <summary>
        /// Map objects by matching property names
        /// </summary>
        public static Tto Map<Tfrom, Tto>(this Tfrom obj)
        {
            return ObjectMapperManager.DefaultInstance
                .GetMapper<Tfrom, Tto>(new DefaultMapConfig()
                .MatchMembers((m1, m2) => m1.ToUpper() == m2.ToUpper()))
                .Map(obj);
        }

        /// <summary>
        /// Map only null objects by matching property names, copying from another object source
        /// </summary>
        public static T MapOnlyNulls<T>(this T objTarget, T objSource)
        {
            if (objTarget == null)
                return Activator.CreateInstance<T>();

            if (objSource == null)
                return objTarget;

            Type t = typeof(T);
            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var sourceValue = prop.GetValue(objSource, null);
                var targetValue = prop.GetValue(objTarget, null);

                //Copy object properties only if target value is null
                if (targetValue == null && sourceValue != null)
                    prop.SetValue(objTarget, sourceValue, null);
            }

            return objTarget;
        }
    }
}