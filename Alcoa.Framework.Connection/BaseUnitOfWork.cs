using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Connection.Factory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace Alcoa.Framework.Connection
{
    /// <summary>
    /// Class to manage transactions between business logic using the Unit of Work pattern
    /// </summary>
    /// <remarks>Can't be debugged</remarks>
    [Browsable(false), DebuggerStepThrough]
    public class BaseUnitOfWork<T> : IBaseUnitOfWork
        where T : DbContext
    {
        [Browsable(false), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T _privateDbContext;

        [Browsable(false), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Tuple<string, object>> _repositoryList { get; set; }

        /// <summary>
        /// Initializes base unit of work using DbContext type name sufix
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public BaseUnitOfWork()
        {
            _repositoryList = new List<Tuple<string, object>>();

            InitializeContext();
        }

        /// <summary>
        /// Initializes base unit of work using Profile name as parameter
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public BaseUnitOfWork(string profileName)
        {
            _repositoryList = new List<Tuple<string, object>>();

            InitializeContext(profileName);
        }

        /// <summary>
        /// Initializes a DbContext
        /// </summary>
        [Browsable(false), DebuggerHidden]
        private void InitializeContext(string profileName = null)
        {
            Database.SetInitializer<T>(null);
            Database.DefaultConnectionFactory = new ConnectionFactory();

            if (_privateDbContext == null)
            {
                var contextType = typeof(T);
                var ctorHasParameters = contextType.GetConstructors().Any(t => t.GetParameters().Any());

                if (!ctorHasParameters && !string.IsNullOrEmpty(profileName))
                    throw new NotImplementedException("DbContext needs a contructor that accepts custom profile names. eg. public DbContext(string profileName) : base(profileName)");

                //if profile name was not provided, gets the type full name
                if (string.IsNullOrWhiteSpace(profileName))
                    profileName = contextType.FullName;

                _privateDbContext = (ctorHasParameters) ?
                    (T)Activator.CreateInstance(typeof(T), profileName) :
                    (T)Activator.CreateInstance<T>();

                _privateDbContext.Configuration.ProxyCreationEnabled = false;
                _privateDbContext.Configuration.LazyLoadingEnabled = false;
            }
        }

        /// <summary>
        /// Initializes db or file repositories using context/domain types
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public IRepository<TType> GetRepository<TType>()
            where TType : Base, IBaseDomain
        {
            var repoName = typeof(TType).Name;
            var repo = _repositoryList.Find(r => r.Item1 == repoName);

            if (repo == null)
            {
                repo = new Tuple<string, object>(repoName, new RepositoryFactory().CreateInstance<TType>(_privateDbContext));
                _repositoryList.Add(repo);
            }

            return (IRepository<TType>)repo.Item2;
        }

        /// <summary>
        /// Commit all changes to Repositories
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public void Commit()
        {
            if (_privateDbContext != null)
            {
                _privateDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Rollback tracked changes in all Repositories
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public void Rollback()
        {
            if (_privateDbContext != null)
            {
                _privateDbContext.ChangeTracker
                    .Entries()
                    .AsParallel()
                    .ForAll(entry => entry.State = EntityState.Unchanged);
            }
        }

        /// <summary>
        /// Executes a DbCommand that returns only affected rows
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public int ExecuteNonQuery(string sqlCommand, Hashtable parameters = null, CommandType commandType = CommandType.Text)
        {
            using (var dh = new DatabaseHelper(_privateDbContext.Database.Connection, false))
            {
                return dh.ExecuteNonQuery(sqlCommand, parameters, commandType);
            }
        }

        /// <summary>
        /// Executes a DbCommand that returns a resultset using the provided parameters.
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public DataSet ExecuteDataset(string sqlCommand, Hashtable parameters = null, CommandType commandType = CommandType.Text)
        {
            using (var dh = new DatabaseHelper(_privateDbContext.Database.Connection, false))
            {
                return dh.ExecuteDataset(sqlCommand, parameters, commandType);
            }
        }

        /// <summary>
        /// Executes a DbCommand that returns the first collumn from the first row as an object
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public Ty ExecuteScalar<Ty>(string sqlCommand, Hashtable parameters = null, CommandType commandType = CommandType.Text)
        {
            using (var dh = new DatabaseHelper(_privateDbContext.Database.Connection, false))
            {
                return (Ty)dh.ExecuteScalar(sqlCommand, parameters, commandType);
            }
        }

        /// <summary>
        /// Disposes used resources
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public void Dispose()
        {
            if (_repositoryList != null)
            {
                _repositoryList.AsParallel().ForAll(rl => ((IDisposable)rl.Item2).Dispose());
                _repositoryList = null;
            }

            _privateDbContext.Dispose();
            _privateDbContext = null;
        }
    }
}