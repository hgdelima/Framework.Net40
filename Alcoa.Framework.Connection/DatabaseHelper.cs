using Alcoa.Framework.Connection.Enumerator;
using Alcoa.Framework.Connection.Factory;
using Alcoa.Framework.Connection.Properties;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Alcoa.Framework.Connection
{
    /// <summary>
    /// Class intended to encapsulate common Database functions, filling readers and datasets.
    /// </summary>
    /// <remarks>Can't be debugged</remarks>
    [Browsable(false), DebuggerStepThrough]
    public class DatabaseHelper : IDisposable
    {
        private DbConnection _connection;
        private DbTransaction _transaction;
        private object _transactionLifetime;
        private bool _dispose;

        [Browsable(false), DebuggerHidden]
        public DatabaseHelper(string connectionName)
        {
            _connection = new ConnectionFactory().CreateConnection(connectionName);
            _dispose = true;
        }

        [Browsable(false), DebuggerHidden]
        public DatabaseHelper(string connectionString, DatabaseTypes dbType)
        {
            _connection = new ConnectionFactory().CreateConnection(connectionString, dbType);
            _dispose = true;
        }

        [Browsable(false), DebuggerHidden]
        public DatabaseHelper(DbConnection connection, bool dispose)
        {
            _connection = connection;
            _dispose = dispose;
        }

        /// <summary>
        /// Begins a new DbConnection transaction
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public void BeginTransaction()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            if (_transaction == null)
                _transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);

            if (_transactionLifetime == null)
                _transactionLifetime = _transaction.InitializeLifetimeService();
        }

        /// <summary>
        /// Commits DbConnection transaction
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public void CommitTransaction()
        {
            if (_transaction != null && _transactionLifetime != null)
            {
                _transaction.Commit();
                _transactionLifetime = null;
            }
        }

        /// <summary>
        /// Rollsback DbConnection transaction
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public void RollbackTransaction()
        {
            if (_transaction != null && _transactionLifetime != null)
            {
                _transaction.Rollback();
                _transactionLifetime = null;
            }
        }

        /// <summary>
        /// Execute a DbCommand (that returns no resultset)
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public int ExecuteNonQuery(string commandText, Hashtable commandParameters = null, CommandType commandType = CommandType.Text)
        {
            var result = default(int);

            using (var cmd = CreateCommand(commandText, commandParameters, commandType))
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                result = cmd.ExecuteNonQuery();

                MapOutputParameters(cmd, commandParameters);
            }

            return result;
        }

        /// <summary>
        /// Execute a DbCommand (that returns a resultset)
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public DataSet ExecuteDataset(string commandText, Hashtable commandParameters = null, CommandType commandType = CommandType.Text)
        {
            var dsResult = new DataSet();

            using (var cmd = CreateCommand(commandText, commandParameters, commandType))
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                var da = default(DataAdapter);

                //create the DataAdapter & DataSet
                if (cmd is OracleCommand)
                    da = new OracleDataAdapter((OracleCommand)cmd);
                else if (cmd is SqlCommand)
                    da = new SqlDataAdapter((SqlCommand)cmd);
                else if (cmd is OleDbCommand)
                    da = new OleDbDataAdapter((OleDbCommand)cmd);

                //fill the DataSet using default values for DataTable names, etc.
                da.Fill(dsResult);
            }

            return dsResult;
        }

        /// <summary>
        /// Execute a DbCommand (that returns a 1x1 resultset)
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public object ExecuteScalar(string commandText, Hashtable commandParameters = null, CommandType commandType = CommandType.Text)
        {
            var result = default(object);

            using (var cmd = CreateCommand(commandText, commandParameters, commandType))
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                result = cmd.ExecuteScalar();
            }

            return result;
        }

        /// <summary>
        /// Opens (if necessary) and assigns a connection, transaction, command type and parameters to the provided command.
        /// </summary>
        [Browsable(false), DebuggerHidden]
        private DbCommand CreateCommand(string commandText, Hashtable commandParameters = null, CommandType commandType = CommandType.Text)
        {
            var command = default(DbCommand);

            if (_connection is OracleConnection)
            {
                command = new OracleCommand(commandText.Replace('?', ':'));
                ((OracleCommand)command).BindByName = true;
            }
            else if (_connection is SqlConnection)
                command = new SqlCommand(commandText);
            else if (_connection is OleDbConnection)
                command = new OleDbCommand(commandText);

            //Attribs connection, commandtype and transaction
            command.Connection = _connection;
            command.CommandType = commandType;
            command.Transaction = _transaction;

            //attach the command parameters if they are provided
            if (commandParameters != null)
                AttachParameters(command, commandParameters);

            return command;
        }

        /// <summary>
        /// Used to attach array's of parameters to DbCommands.
        /// </summary>
        [Browsable(false), DebuggerHidden]
        private void AttachParameters(DbCommand command, Hashtable commandParameters)
        {
            //If its a oracle procedure/package
            if (command is OracleCommand &&
                command.CommandType == CommandType.StoredProcedure)
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                //Derive parameter types, directions and sizes from Oracle DB
                OracleCommandBuilder.DeriveParameters((OracleCommand)command);

                if (command.Parameters.Count != commandParameters.Count)
                    throw new ArgumentException(Resources.MsgParameterNumberDiffers);

                foreach (var k in commandParameters.Keys)
                {
                    var key = k.ToString().ToUpper();

                    if (!command.Parameters.Contains(key))
                        throw new IndexOutOfRangeException(string.Format(Resources.MsgParameterNotFound, key));

                    command.Parameters[key].Value = (commandParameters[key] ?? DBNull.Value);
                }
            }
            else
            {
                foreach (var k in commandParameters.Keys)
                {
                    var key = k.ToString().ToUpper();

                    //Check for null values and replaces by DBNull
                    var value = (commandParameters[key] ?? DBNull.Value);

                    if (command is OracleCommand)
                        command.Parameters.Add(new OracleParameter(key, value));
                    else if (command is SqlCommand)
                        command.Parameters.Add(new SqlParameter(key, value));
                    else if (command is OleDbCommand)
                        command.Parameters.Add(new OleDbParameter(key, value));
                }
            }
        }

        /// <summary>
        /// Map returning values from executed command to parameters
        /// </summary>
        [Browsable(false), DebuggerHidden]
        private void MapOutputParameters(DbCommand cmd, Hashtable commandParameters)
        {
            if (commandParameters != null)
            {
                //Clones parameters list just to use on ForEach
                var parametersClone = (Hashtable)commandParameters.Clone();

                foreach (var k in parametersClone.Keys)
                {
                    var key = k.ToString().ToUpper();
                    var paramValue = commandParameters[key] ?? DBNull.Value;
                    var returnValue = cmd.Parameters[key].Value;

                    //If values differs, attrib return value to parameters list
                    if (paramValue != returnValue)
                        commandParameters[key] = returnValue;
                }
            }
        }

        /// <summary>
        /// Dispose all internal resources
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public void Dispose()
        {
            if (_transaction != null &&
                _transactionLifetime != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();

                _transaction = null;
                _transactionLifetime = null;
            }

            if (_connection.State == ConnectionState.Open)
                _connection.Close();

            if (_dispose)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}