using System;
using System.Reflection;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps manipulate assemblies operations
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Verifies if a caller assembly matchs an assembly name
        /// </summary>
        public static void MatchAssemblyName(Assembly callerAssembly, string assemblyNameToMatch)
        {
            if (!callerAssembly.FullName.Contains(assemblyNameToMatch))
                throw new InvalidOperationException("Invalid caller assembly, this assembly can only be accessed by " + assemblyNameToMatch);
        }
    }
}