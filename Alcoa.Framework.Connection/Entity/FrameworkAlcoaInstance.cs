using Alcoa.Framework.Common;
using Alcoa.Framework.Connection.Enumerator;
using Alcoa.Framework.Connection.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Alcoa.Framework.Connection.Entity
{
    /// <summary>
    /// Class to authorize machines for specified database instances
    /// </summary>
    [DebuggerStepThrough]
    internal static class FrameworkAlcoaInstance
    {
        private static EnvironmentTypes Environment { get; set; }
        private static List<string> _deniedInstances_ { get; set; }

        static FrameworkAlcoaInstance()
        {
            try
            {
                //Searchs for instance file inside some windows folders
                var instanceFullPath = FileHelper.SearchFileFullPath(Resources.VaultPath, Resources.InstanceFileName, Resources.VaultFolderToSearch);

                XDocument instanceFile = XDocument.Load(instanceFullPath);

                var environment = instanceFile.Descendants("Current").Select(e => e.Value.ToUpper()).FirstOrDefault();
                if (environment != null)
                    Environment = (EnvironmentTypes)Convert.ToChar(environment);

                _deniedInstances_ = instanceFile.Descendants("DenyInstance").Select(e => e.Value.ToUpper()).ToList();
            }
            catch
            {
            }
        }

        internal static void ValidateInstanceAuthorization(string instanceName)
        {
            if (_deniedInstances_.Count <= 0)
                throw new FileNotFoundException(Resources.MsgInstanceVaultFileNotFound);

            if (Environment == EnvironmentTypes.DEV ||
                Environment == EnvironmentTypes.QA ||
                Environment == EnvironmentTypes.HLG)
            {
                var invalidInstance = _deniedInstances_.Exists(di => di.ToUpper() == instanceName.ToUpper());

                if (invalidInstance)
                    throw new UnauthorizedAccessException(string.Format(Resources.MsgUnauthorizedAccessToInstance, Environment, instanceName));
            }
        }
    }
}
