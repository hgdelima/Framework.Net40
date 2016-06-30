using Alcoa.Framework.Common;
using Alcoa.Framework.Connection.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Alcoa.Framework.Connection.Entity
{
    /// <summary>
    /// Class to initialize framework Active Directory configurations
    /// </summary>
    [DebuggerStepThrough]
    internal static class FrameworkAlcoaActiveDirectory
    {
        private static List<ActiveDirectory> _activeDirectoryConfigs { get; set; }

        /// <summary>
        /// Reads VaultFolder and initialize AD data
        /// </summary>
        static FrameworkAlcoaActiveDirectory()
        {
            try
            {
                //Searchs for vault file inside some windows folders
                var fileFullPath = FileHelper.SearchFileFullPath(Resources.VaultPath, Resources.ActiveDirectoryFileName, Resources.VaultFolderToSearch);

                XDocument vaultFile = XDocument.Load(fileFullPath);

                //Read and loads connection data
                _activeDirectoryConfigs = vaultFile.Descendants("Domain")
                    .Select(ad => new ActiveDirectory
                    {
                        SearchPriority = ad.Attribute("searchPriority") != null ? Convert.ToInt16(ad.Attribute("searchPriority").Value) : default(short?),
                        Domain = ad.Element("name") != null ? ad.Element("name").Value : string.Empty,
                        OrganizationUnit = ad.Element("organizationUnit") != null ? ad.Element("organizationUnit").Value : string.Empty,
                        RootOrganizationUnit = ad.Element("rootOrganizationUnit") != null ? ad.Element("rootOrganizationUnit").Value : string.Empty,
                        ServiceUser = ad.Element("serviceUser") != null ? ad.Element("serviceUser").Value : string.Empty,
                        ServicePassword = ad.Element("servicePassword") != null ? ad.Element("servicePassword").Value : string.Empty,
                    })
                    .OrderBy(ad => ad.SearchPriority)
                    .ToList();
            }
            catch
            {
            }
        }

        internal static List<PrincipalContext> GetPrincipalContexts()
        {
            if (_activeDirectoryConfigs.Count <= 0)
                throw new FileNotFoundException(Resources.MsgActiveDirectoryVaultFileNotFound);

            var _principalContextsDic = new Dictionary<short, PrincipalContext>();
            foreach (var ad in _activeDirectoryConfigs)
            {
                var pc = string.IsNullOrEmpty(ad.OrganizationUnit) ?
                    new PrincipalContext(ContextType.Domain, ad.Domain, ad.RootOrganizationUnit, ContextOptions.Negotiate) :
                    new PrincipalContext(ContextType.Domain, ad.Domain, ad.OrganizationUnit, ContextOptions.Negotiate);

                _principalContextsDic.Add(ad.SearchPriority.GetValueOrDefault(), pc);
            }

            //Ensures that principal context list will have the right search priority
            var orderedDic = _principalContextsDic.OrderBy(k => k.Key).Select(od => od.Value);
            return new List<PrincipalContext>(orderedDic);
        }
    }
}
