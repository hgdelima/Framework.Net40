using Alcoa.Framework.Connection.Entity;
using Alcoa.Framework.Connection.Enumerator;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;

namespace Alcoa.Framework.Connection.Factory
{
    /// <summary>
    /// Class to factory connections for internal objects
    /// </summary>
    /// <remarks>Can't be debugged</remarks>
    [Browsable(false), DebuggerStepThrough]
    internal class ConnectionFactory : IDbConnectionFactory
    {
        private static Dictionary<string, object> InvalidUserOrPassConns;

        [Browsable(false), DebuggerHidden]
        public ConnectionFactory()
        {
            if (InvalidUserOrPassConns == null)
                InvalidUserOrPassConns = new Dictionary<string, object>();
        }

        [Browsable(false), DebuggerHidden]
        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            var _dbConnection = default(DbConnection);

            if (InvalidUserOrPassConns.ContainsKey(nameOrConnectionString))
                throw (Exception)InvalidUserOrPassConns[nameOrConnectionString];

            try
            {
                //Creates DbConnection using the dbcontext type name
                _dbConnection = FrameworkAlcoaVault.GetConnectionByContext(nameOrConnectionString);

                //Validates and authorizes machines for specified database instances
                FrameworkAlcoaInstance.ValidateInstanceAuthorization(_dbConnection.DataSource);

                //Test connection before use
                _dbConnection.Open();
                _dbConnection.Close();
            }
            catch (OracleException ex)
            {
                //Only adds to invalid dictionary if error is for Invalid User or Password
                if (ex.ErrorCode == 1017)
                    InvalidUserOrPassConns[nameOrConnectionString] = ex;

                if (_dbConnection.State != ConnectionState.Closed)
                    _dbConnection.Close();

                _dbConnection.Dispose();
                _dbConnection = null;

                throw ex;
            }

            return _dbConnection;
        }

        [Browsable(false), DebuggerHidden]
        public DbConnection CreateConnection(string nameOrConnectionString, DatabaseTypes dbTypes)
        {
            var _dbConnection = default(DbConnection);

            if (InvalidUserOrPassConns.ContainsKey(nameOrConnectionString))
                throw (Exception)InvalidUserOrPassConns[nameOrConnectionString];

            try
            {
                var profile = new Profile 
                { 
                    ConnectionString = nameOrConnectionString,
                    DatabaseType = dbTypes
                };

                //Creates DbConnection using the dbcontext type name
                _dbConnection = FrameworkAlcoaVault.GetConnectionByProfile(profile);

                //Validates and authorizes machines for specified database instances
                FrameworkAlcoaInstance.ValidateInstanceAuthorization(_dbConnection.DataSource);

                //Test connection before use
                _dbConnection.Open();
                _dbConnection.Close();
            }
            catch (OracleException ex)
            {
                //Only adds to invalid dictionary if error is for Invalid User or Password
                if (ex.ErrorCode == 1017)
                    InvalidUserOrPassConns[nameOrConnectionString] = ex;

                if (_dbConnection.State != ConnectionState.Closed)
                    _dbConnection.Close();

                _dbConnection.Dispose();
                _dbConnection = null;

                throw ex;
            }

            return _dbConnection;
        }
    }
}