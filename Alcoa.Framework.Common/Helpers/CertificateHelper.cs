using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps handle certificate files
    /// </summary>
    public static class CertificateHelper
    {
        /// <summary>
        /// Get certificate at specific store place in local machine
        /// </summary>
        public static X509Certificate2 GetCertificateAtStore(string certificateSubjectName, StoreName storeName)
        {
            X509Certificate2Collection certificates = null;
            var store = new X509Store(storeName, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            try
            {
                certificates = store.Certificates;
                var certs = certificates.OfType<X509Certificate2>()
                    .Where(x => x.SubjectName.Name != null && x.SubjectName.Name.Equals(certificateSubjectName, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (!certs.Any() || certs.Count > 1)
                    return default(X509Certificate2);

                return certs.First();
            }
            finally
            {
                if (certificates != null)
                {
                    foreach (var cert in certificates)
                        cert.Reset();
                }

                store.Close();
            }
        }
    }
}