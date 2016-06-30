using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Connection.Repository;

namespace Alcoa.Framework.Connection.Factory
{
    /// <summary>
    /// Class to factory Active Directory Repository, as single or many instances
    /// </summary>
    public class ActiveDirectoryFactory
    {
        private static IActiveDirectoryRepository _adRepository { get; set; }

        /// <summary>
        /// Active Directory Factory 
        /// </summary>
        public ActiveDirectoryFactory()
        {
        }

        /// <summary>
        /// Gets an existent instance or creates a new one
        /// </summary>
        public IActiveDirectoryRepository GetSingleInstance()
        {
            return _adRepository ?? (_adRepository = new ActiveDirectoryRepository());
        }

        /// <summary>
        /// Creates a new instance for active directory repository
        /// </summary>
        public IActiveDirectoryRepository CreateInstance()
        {
            return new ActiveDirectoryRepository();
        }
    }
}