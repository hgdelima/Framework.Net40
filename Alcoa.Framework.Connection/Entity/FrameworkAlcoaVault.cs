using Alcoa.Framework.Common;
using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Connection.Enumerator;
using Alcoa.Framework.Connection.Properties;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Alcoa.Framework.Connection.Entity
{
    /// <summary>
    /// Validates and initiates framework vault connections
    /// </summary>
    [Browsable(false), DebuggerStepThrough]
    internal static class FrameworkAlcoaVault
    {
        private static List<Profile> _profiles_ { get; set; }
        private static Pattern _pattern { get; set; }

        private const string _contextWord = "Context";
        private const char _contextSeparator = '_';
        private static string _password = string.Empty;

        /// <summary>
        /// Read Framework Vault file and initializes database configurations
        /// </summary>
        [Browsable(false), DebuggerHidden]
        static FrameworkAlcoaVault()
        {
            try
            {
                //Searchs for vault file inside some windows folders
                var vaultFullPath = FileHelper.SearchFileFullPath(Resources.VaultPath, Resources.VaultFileName, Resources.VaultFolderToSearch);

                //Combine and generate vault password to decrypt connection strings
                var fixes = CommonResource.GetString("PassNumbers") + CommonResource.GetString("PassSpecialChars");
                _password = fixes + CommonResource.GetString("PassText") + fixes;

                XDocument vaultFile = XDocument.Load(vaultFullPath);

                //Read and loads connection data
                _profiles_ = vaultFile.Descendants("Profile")
                    .Select(e => new Profile
                    {
                        Name = e.FirstAttribute.Value.ToUpper(),
                        DatabaseType = (DatabaseTypes)Enum.Parse(typeof(DatabaseTypes), e.Parent.Name.ToString()),
                        ConnectionString = e.Value,
                        UsePattern = e.LastAttribute.Value.ToBool()
                    })
                    .ToList();

                //Read and loads pattern data
                _pattern = vaultFile.Descendants("Pattern")
                    .Select(e => new Pattern { PatternValue = e.Value })
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
        }

        [Browsable(false), DebuggerHidden]
        internal static DbConnection GetConnectionByContext(string contextOrProfileName)
        {
            if (string.IsNullOrEmpty(contextOrProfileName))
                throw new ArgumentNullException(Resources.MsgContextCantBeNull);

            //Gets profile and decrypts connection string
            var profile = GetProfile(contextOrProfileName);
            profile.ConnectionString = GetConnectionString(profile);

            return GetConnectionByProfile(profile);
        }

        [Browsable(false), DebuggerHidden]
        internal static DbConnection GetConnectionByProfile(Profile profile)
        {
            if (profile == null || string.IsNullOrEmpty(profile.ConnectionString))
                throw new ArgumentNullException(Resources.MsgConnectionStringNotFoundForProfile);

            var connection = default(DbConnection);

            switch (profile.DatabaseType)
            {
                case DatabaseTypes.Access:
                    connection = new OleDbConnection(profile.ConnectionString);
                    break;

                case DatabaseTypes.Oracle:
                    connection = new OracleConnection(profile.ConnectionString);
                    break;

                case DatabaseTypes.SQLServer:
                    connection = new SqlConnection(profile.ConnectionString);
                    break;

                default:
                    connection = new OracleConnection(profile.ConnectionString);
                    break;
            }

            return connection;
        }

        [Browsable(false), DebuggerHidden]
        private static Profile GetProfile(string contextFullName)
        {
            if (_profiles_.Count <= 0)
                throw new FileNotFoundException(Resources.MsgFrameworkVaultFileNotFound);

            //Gets profile name and searchs in Vault class
            var profileName = contextFullName.Replace(_contextWord, _contextSeparator.ToString()).Split(_contextSeparator).Last().ToUpper();
            var profile = _profiles_.FirstOrDefault(c => c.Name.ToUpper() == profileName.ToUpper());

            if (profile == default(Profile))
                throw new ObjectNotFoundException(string.Format(Resources.MsgProfileNotFound, profileName));

            if (string.IsNullOrWhiteSpace(profile.ConnectionString))
                throw new ObjectNotFoundException(string.Format(Resources.MsgConnectionStringNotFoundForProfile, profileName));

            return new Profile(profile);
        }

        [Browsable(false), DebuggerHidden]
        private static string GetConnectionString(Profile profile)
        {
            //Initializes and decrypts profile connection string
            string connectionString = CryptographHelper.RijndaelDecrypt(profile.ConnectionString, _password);

            //Mounts vault pattern to be attached in connection string
            if (profile.UsePattern)
            {
                var passwordPattern = CryptographHelper.RijndaelDecrypt(_pattern.PatternValue, _password).ToUpper();
                passwordPattern = _pattern.PatternOptions
                    .Aggregate(passwordPattern, (pattern, option) =>
                        pattern.Replace(option.Key, (string.IsNullOrWhiteSpace(option.Value) ? profile.Name : option.Value)));

                connectionString += passwordPattern;
            }

            return connectionString;
        }
    }
}
