using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Common.Entity
{
    /// <summary>
    /// Adds object types in data contracts to all services
    /// </summary>
    public class ServiceKnownTypesAttribute
    {
        private static readonly List<Type> _knowTypes = new List<Type>();
        private static readonly IEnumerable<string> assembliesNamespaces = new Collection<string> { @"Entity", @"Contract" };
        private static readonly IEnumerable<Type> _assembliesTypes = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a =>
            {
                var assemblyNameArray = a.GetName().Name.Split('.');
                return assembliesNamespaces.Contains(assemblyNameArray.LastOrDefault());
            })
            .SelectMany(a => a.GetTypes().Where(ea => ea.IsClass && ea.IsGenericType == false && ea.IsGenericTypeDefinition == false));

        /// <summary>
        /// Adds knew object types for serialization
        /// </summary>
        public static IEnumerable<Type> GetServiceTypes(ICustomAttributeProvider provider)
        {
            var knowTypesAdded = _knowTypes.Intersect(_assembliesTypes);

            if (!knowTypesAdded.Any())
                _knowTypes.AddRange(_assembliesTypes);

            return _knowTypes;
        }
    }
}