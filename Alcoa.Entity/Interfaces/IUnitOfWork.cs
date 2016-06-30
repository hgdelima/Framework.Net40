using Alcoa.Entity.Entity;
using Alcoa.Entity.Repository;

namespace Alcoa.Entity.Interfaces
{
    /// <summary>
    /// Unit of Work interface that encapsulates unit of work base to hide data access flow.
    /// </summary>
    public interface IUnitOfWork : IBaseUnitOfWork
    {
        /// <summary>
        /// Get or creates a repository instance 
        /// </summary>
        /// <typeparam name="T">Implemented repository</typeparam>
        BaseRepository<R> GetRepository<R>() where R : Base, IBaseDomain;

        /// <summary>
        /// Get or creates an Active Directory repository instance
        /// </summary>
        IActiveDirectoryRepository GetRepositoryActiveDirectory();
    }
}