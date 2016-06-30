using System.Collections.Generic;
using Alcoa.Entity.Entity;
using System;

namespace Alcoa.Entity.Interfaces
{
    /// <summary>
    /// Defines an interface for common Active Directory operations.
    /// </summary>
    public interface IActiveDirectoryRepository : IDisposable
    {
        /// <summary>
        /// Gets a user on Active Directory
        /// </summary>
        /// <param name="userIdentity">The username to get</param>
        /// <param name="loadSubProperties">Load all properties including groups (Slow)</param>
        BaseActiveDirectoryUser GetUser(string userIdentity, bool loadSubProperties);

        /// <summary>
        /// Gets a user on Active Directory
        /// </summary>
        /// <param name="userIdentity">The username to get</param>
        /// <param name="loadSubProperties">Load all properties including groups (Slow)</param>
        /// <param name="loadDirectReports">Load direct reports (Slow)</param>
        BaseActiveDirectoryUser GetUser(string userIdentity, bool loadSubProperties, bool loadThirdPartners);

        /// <summary>
        /// Gets a list of users on Active Directory
        /// </summary>
        /// <param name="nameOrSamAccount">Searchs in object Name and SamAccountName</param>
        /// <param name="domain">Specific domain name for faster searchs</param>
        List<BaseActiveDirectoryUser> SearchUsers(string nameOrSamAccount, string domain = null);

        /// <summary>
        /// Gets a certain group on Active Directory
        /// </summary>
        /// <param name="groupIdentity">The group to get</param>
        /// <param name="loadSubProperties">Load all properties including users (Slow)</param>
        BaseActiveDirectoryGroup GetGroup(string groupIdentity, bool loadSubProperties);

        /// <summary>
        /// Gets a certain group on Active Directory
        /// </summary>
        /// <param name="nameOrSamAccount">Searchs in object Name and SamAccountName</param>
        /// <param name="domain">Specific domain name for faster searchs</param>
        List<BaseActiveDirectoryGroup> SearchGroups(string nameOrSamAccount, string domain = null);

        /// <summary>
        /// Get all domains within federated network
        /// </summary>
        List<BaseNetworkDomain> GetDomainList();

        /// <summary>
        /// Validates the username and password of a given user
        /// </summary>
        /// <param name="userIdentity">The username to validate</param>
        /// <param name="password">The password of the username to validate</param>
        bool ValidateUserCredential(string userIdentity = null, string password = null);

        /// <summary>
        /// Sets the user password
        /// </summary>
        /// <param name="userIdentity">The username to set</param>
        /// <param name="newPassword">The new password to use</param>
        string SetUserPassword(string userIdentity, string newPassword);

        /// <summary>
        /// Force expire password of a user
        /// </summary>
        /// <param name="userIdentity">The username to expire the password</param>
        void ExpireUserPassword(string userIdentity);

        /// <summary>
        /// Unlocks a locked user account
        /// </summary>
        /// <param name="userIdentity">The username to unlock</param>
        void UnlockUserAccount(string userIdentity);
    }
}