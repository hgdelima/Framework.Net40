using Alcoa.Framework.Connection.Enumerator;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;

namespace Alcoa.Framework.Connection.Entity
{
    [Serializable]
    [Browsable(false), DebuggerStepThrough]
    internal class Profile
    {
        public Profile()
        {
        }

        public Profile(Profile profile)
        {
            Name = profile.Name;
            ConnectionString = profile.ConnectionString;
            UsePattern = profile.UsePattern;
            DatabaseType = profile.DatabaseType;
            DbConn = profile.DbConn;
        }

        internal string Name { get; set; }

        internal string ConnectionString { get; set; }

        internal bool UsePattern { get; set; }

        internal DatabaseTypes DatabaseType { get; set; }

        internal DbConnection DbConn { get; set; }
    }
}