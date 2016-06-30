using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Connection.Repository;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;

namespace Alcoa.Framework.Connection.Factory
{
    /// <summary>
    /// Class to factory Database or File repositories
    /// </summary>
    /// <remarks>Can't be debugged</remarks>
    [Browsable(false), DebuggerStepThrough]
    internal class RepositoryFactory
    {
        /// <summary>
        /// Initializes factory using BaseUnitOfWork object
        /// </summary>
        [Browsable(false), DebuggerHidden]
        internal RepositoryFactory()
        {
        }

        /// <summary>
        /// Creates a connected repository instance
        /// </summary>
        [Browsable(false), DebuggerHidden]
        internal IRepository<TType> CreateInstance<TType>(DbContext dbContext)
            where TType : Base, IBaseDomain
        {
            IRepository<TType> repo;

            if (dbContext == null)
                repo = Activator.CreateInstance<FileRepository<TType>>();
            else
                repo = (DbRepository<TType>)Activator.CreateInstance(typeof(DbRepository<TType>), dbContext);

            return repo;
        }
    }
}