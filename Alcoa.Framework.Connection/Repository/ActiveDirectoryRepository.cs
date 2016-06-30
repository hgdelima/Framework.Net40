using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Common;
using Alcoa.Framework.Common.Entity;
using Alcoa.Framework.Connection.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;

namespace Alcoa.Framework.Connection.Repository
{
    /// <summary>
    /// Class to access Active Directory objects (Users and Groups)
    /// </summary>
    /// <remarks>Can't be debugged</remarks>
    [Browsable(false), DebuggerStepThrough]
    internal class ActiveDirectoryRepository : IActiveDirectoryRepository
    {
        //LDAP variables
        internal const string groupDirectoryKey = "memberOf";
        internal const string officeDirectoryKey = "physicalDeliveryOfficeName";
        internal const string directReportsKey = "directReports";
        internal const string managedByKey = "managedBy";
        internal const string managerKey = "manager";
        internal const string whenCreatedKey = "whenCreated";

        [Browsable(false), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<PrincipalContext> _principalContexts { get; set; }

        /// <summary>
        /// Loads all principal contexts
        /// </summary>
        [Browsable(false), DebuggerHidden]
        internal ActiveDirectoryRepository()
        {
            _principalContexts = FrameworkAlcoaActiveDirectory.GetPrincipalContexts();
        }

        #region Public Methods

        public BaseActiveDirectoryUser GetUser(string userIdentity, bool loadSubProperties)
        {
            return GetUser(userIdentity, loadSubProperties, false);
        }

        public BaseActiveDirectoryUser GetUser(string userIdentity, bool loadSubProperties, bool loadThirdPartners)
        {
            var user = new BaseActiveDirectoryUser();
            var userPrincipal = GetUserPrincipal(userIdentity);

            if (userPrincipal != null)
                user = MapUserPrincipalToUser(userPrincipal, loadSubProperties, loadThirdPartners);

            return user;
        }

        public List<BaseActiveDirectoryUser> SearchUsers(string nameOrSamAccount, string domain = null)
        {
            var userList = new List<BaseActiveDirectoryUser>();
            var userPrincipalList = new List<UserPrincipal>();
            var domainsToSearch = _principalContexts.Where(pc => pc.Name == domain || domain == null);

            foreach (var ds in domainsToSearch)
            {
                //Searchs on name field
                var userPrincipal = new UserPrincipal(ds) { Name = "*" + nameOrSamAccount + "*" };
                var searcher = new PrincipalSearcher(userPrincipal);

                //Add results as UserPrincipal
                userPrincipalList.AddRange(searcher.FindAll().Cast<UserPrincipal>());

                //Searchs on SamAccount field
                userPrincipal = new UserPrincipal(ds) { SamAccountName = "*" + nameOrSamAccount + "*" };
                searcher = new PrincipalSearcher(userPrincipal);

                //Add results as UserPrincipal
                userPrincipalList.AddRange(searcher.FindAll().Cast<UserPrincipal>());
            }

            //Filter distincts groupPrincipals by SamAccountName
            userPrincipalList = userPrincipalList
                .Distinct(new EqualBy<UserPrincipal>((x, y) => x.SamAccountName == y.SamAccountName))
                .ToList();

            //Casts all groupPrincipal to GroupBase
            userPrincipalList.AsParallel().ForAll(up => userList.Add(MapUserPrincipalToUser(up, false, false)));

            return userList;
        }

        public BaseActiveDirectoryGroup GetGroup(string groupIdentity, bool loadSubProperties)
        {
            var adGroup = new BaseActiveDirectoryGroup();

            foreach (var pc in _principalContexts)
            {
                var groupPrincipal = GroupPrincipal.FindByIdentity(pc, groupIdentity);

                if (groupPrincipal != null)
                {
                    adGroup = MapGroupPrincipalToGroup(groupPrincipal, loadSubProperties);
                    break;
                }
            }

            return adGroup;
        }

        public List<BaseActiveDirectoryGroup> SearchGroups(string nameOrSamAccount, string domain = null)
        {
            var adGroupList = new List<BaseActiveDirectoryGroup>();
            var groupPrincipalList = new List<GroupPrincipal>();
            var domainsToSearch = _principalContexts.Where(pc => pc.Name == domain || domain == null);

            foreach (var ds in domainsToSearch)
            {
                //Searchs for name
                var groupPrincipal = new GroupPrincipal(ds) { Name = "*" + nameOrSamAccount + "*" };
                var searcher = new PrincipalSearcher(groupPrincipal);

                //Add results as GroupPrincipal
                groupPrincipalList.AddRange(searcher.FindAll().Cast<GroupPrincipal>());

                //Searchs on SamAccount Name
                groupPrincipal = new GroupPrincipal(ds) { SamAccountName = "*" + nameOrSamAccount + "*" };
                searcher = new PrincipalSearcher(groupPrincipal);

                //Add results as GroupPrincipal
                groupPrincipalList.AddRange(searcher.FindAll().Cast<GroupPrincipal>());
            }

            //Filter distincts groupPrincipals by SamAccountName
            groupPrincipalList = groupPrincipalList
                .Distinct(new EqualBy<GroupPrincipal>((x, y) => x.SamAccountName == y.SamAccountName))
                .ToList();

            //Casts all groupPrincipal to GroupBase
            groupPrincipalList.AsParallel().ForAll(gp => adGroupList.Add(MapGroupPrincipalToGroup(gp, false)));

            return adGroupList;
        }

        /// <summary>
        /// Lists all available domains
        /// </summary>
        public List<BaseNetworkDomain> GetDomainList()
        {
            return _principalContexts.Select(pc => new BaseNetworkDomain
            {
                DomainName = pc.Name,
                Container = pc.Container
            })
            .ToList();
        }

        /// <summary>
        /// Validates user credentials against all domains
        /// </summary>
        /// <param name="userIdentity">Username or SamAccountName</param>
        /// <param name="password">User password</param>
        public bool ValidateUserCredential(string userIdentity = null, string password = null)
        {
            var validCredential = default(bool);

            foreach (var pc in _principalContexts)
            {
                validCredential = pc.ValidateCredentials(userIdentity, password, ContextOptions.Negotiate);

                if (validCredential)
                    break;
            }

            return validCredential;
        }

        public string SetUserPassword(string userIdentity, string newPassword)
        {
            try
            {
                UserPrincipal oUserPrincipal = GetUserPrincipal(userIdentity);
                oUserPrincipal.SetPassword(newPassword);

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void ExpireUserPassword(string userIdentity)
        {
            UserPrincipal oUserPrincipal = GetUserPrincipal(userIdentity);
            oUserPrincipal.ExpirePasswordNow();
            oUserPrincipal.Save();
        }

        public void UnlockUserAccount(string userIdentity)
        {
            UserPrincipal oUserPrincipal = GetUserPrincipal(userIdentity);
            oUserPrincipal.UnlockAccount();
            oUserPrincipal.Save();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an user on Active Directory using SamAccountName or UserPrincipalName or DistinguishedName
        /// </summary>
        private UserPrincipal GetUserPrincipal(string sUserName)
        {
            var userPrincipal = default(UserPrincipal);

            foreach (var pc in _principalContexts)
            {
                userPrincipal = UserPrincipal.FindByIdentity(pc, sUserName);

                if (userPrincipal != null)
                    break;
            }

            return userPrincipal;
        }

        /// <summary>
        /// Gets an user on Active Directory using identity type, like DistinguishedName, SamAccountName, Name
        /// Ex.: CN=Boas\\, Rogerio B.V. - sadmin,OU=Applic Administrators,OU=SOA,DC=soa,DC=Alcoa,DC=com
        /// </summary>
        private UserPrincipal GetUserPrincipal(string sUserName, IdentityType identityType)
        {
            var userPrincipal = default(UserPrincipal);

            foreach (var pc in _principalContexts)
            {
                userPrincipal = UserPrincipal.FindByIdentity(pc, identityType, sUserName.Trim());

                if (userPrincipal != null)
                    break;
            }

            return userPrincipal;
        }

        /// <summary>
        /// Maps a given user principal to a new user object
        /// </summary>
        private BaseActiveDirectoryUser MapUserPrincipalToUser(UserPrincipal userPrincipal, bool loadSubProperties, bool loadDirectReports)
        {
            // Creates the user
            var user = new BaseActiveDirectoryUser(
                userPrincipal.EmployeeId,
                ConvertSidToString(userPrincipal.Sid),
                userPrincipal.Context.Name,
                userPrincipal.SamAccountName,
                userPrincipal.EmailAddress,
                userPrincipal.DisplayName,
                userPrincipal.AccountExpirationDate,
                userPrincipal.GivenName,
                userPrincipal.MiddleName,
                userPrincipal.Surname,
                userPrincipal.VoiceTelephoneNumber,
                userPrincipal.Enabled);

            //Loads subproperties
            if (loadSubProperties || loadDirectReports)
            {
                //Loads user groups
                var directoryEntry = (DirectoryEntry)userPrincipal.GetUnderlyingObject();
                var groupArray = directoryEntry.Properties[groupDirectoryKey];

                if (groupArray != null)
                {
                    var groups = (object[])groupArray.Value;

                    groups.AsParallel().ForAll(g =>
                        {
                            var adGroup = g.ToString().Substring(3).Split(',').FirstOrDefault();
                            user.ActiveDirectoryGroups.Add(new BaseActiveDirectoryGroup { Id = adGroup, NameOrDescription = adGroup });
                        });
                }

                //Checks if user account is locked and Loads additional data.
                user.UserExtraInfo.IsLocked = userPrincipal.IsAccountLockedOut();
                user.UserExtraInfo.Office = (string)directoryEntry.InvokeGet(officeDirectoryKey);

                //Loads AD manager data
                user.ActiveDirectoryManager.Login = (string)directoryEntry.InvokeGet(managerKey);

                if (!string.IsNullOrEmpty(user.ActiveDirectoryManager.Login))
                {
                    var manager = GetUserPrincipal(user.ActiveDirectoryManager.Login, IdentityType.DistinguishedName);
                    if (manager != null)
                        user.ActiveDirectoryManager = new BaseActiveDirectoryUser(
                                                        manager.EmployeeId,
                                                        ConvertSidToString(manager.Sid),
                                                        manager.Context.Name,
                                                        manager.SamAccountName,
                                                        manager.EmailAddress,
                                                        manager.DisplayName,
                                                        manager.AccountExpirationDate,
                                                        manager.GivenName,
                                                        manager.MiddleName,
                                                        manager.Surname,
                                                        manager.VoiceTelephoneNumber,
                                                        manager.Enabled);
                }

                //Loads Direct Reports And Manager
                if (loadDirectReports)
                {
                    var relatedUsers = directoryEntry.Properties[directReportsKey];

                    if (relatedUsers != null && relatedUsers.Value != null)
                    {
                        // Gets the reports
                        if (relatedUsers.Value is string)
                        {
                            var pUser = GetUserPrincipal(relatedUsers.Value.ToString(), IdentityType.DistinguishedName);

                            if (pUser != null)
                                user.ActiveDirectoryThirdPartners.Add(new BaseActiveDirectoryUser(
                                                                        pUser.EmployeeId,
                                                                        ConvertSidToString(pUser.Sid),
                                                                        pUser.Context.Name,
                                                                        pUser.SamAccountName,
                                                                        pUser.EmailAddress,
                                                                        pUser.DisplayName,
                                                                        pUser.AccountExpirationDate,
                                                                        pUser.GivenName,
                                                                        pUser.MiddleName,
                                                                        pUser.Surname,
                                                                        pUser.VoiceTelephoneNumber,
                                                                        pUser.Enabled));
                        }
                        else
                        {
                            var usersArray = (object[])relatedUsers.Value;

                            foreach (var dn in usersArray)
                            {
                                var pUser = GetUserPrincipal(dn.ToString(), IdentityType.DistinguishedName);

                                if (pUser != null)
                                    user.ActiveDirectoryThirdPartners.Add(new BaseActiveDirectoryUser(
                                                                            pUser.EmployeeId,
                                                                            ConvertSidToString(pUser.Sid),
                                                                            pUser.Context.Name,
                                                                            pUser.SamAccountName,
                                                                            pUser.EmailAddress,
                                                                            pUser.DisplayName,
                                                                            pUser.AccountExpirationDate,
                                                                            pUser.GivenName,
                                                                            pUser.MiddleName,
                                                                            pUser.Surname,
                                                                            pUser.VoiceTelephoneNumber,
                                                                            pUser.Enabled));
                            }
                        }
                    }
                }
            }

            return user;
        }

        /// <summary>
        /// Maps a given group principal to a new group object
        /// </summary>
        private BaseActiveDirectoryGroup MapGroupPrincipalToGroup(GroupPrincipal groupPrincipal, bool loadSubProperties)
        {
            var adGroup = new BaseActiveDirectoryGroup
            {
                Id = ConvertSidToString(groupPrincipal.Sid),
                NameOrDescription = groupPrincipal.Name,
            };

            //Check to load subproperties
            if (loadSubProperties)
            {
                //Load groups additional data
                var directoryEntry = (DirectoryEntry)groupPrincipal.GetUnderlyingObject();

                adGroup.Path = directoryEntry.Path;
                adGroup.CreationDate = (DateTime?)directoryEntry.InvokeGet(whenCreatedKey);
                adGroup.Owner = (string)directoryEntry.InvokeGet(managedByKey);

                if (!string.IsNullOrEmpty(adGroup.Owner))
                    adGroup.Owner = adGroup.Owner.Substring(3, (adGroup.Owner.IndexOf("OU=") - 3)).RemoveSpecialChars();

                //Load group users
                var groupUsersList = groupPrincipal.GetMembers()
                    .Where(m => m.GetType() == typeof(UserPrincipal))
                    .Cast<UserPrincipal>()
                    .Select(gul => new BaseIdentification
                    {
                        Id = ConvertSidToString(gul.Sid),
                        NameOrDescription = gul.SamAccountName
                    });

                //Add users inside current AD group
                adGroup.Users.AddRange(groupUsersList);
            }

            return adGroup;
        }

        /// <summary>
        /// Convert Sid from SecurityIdentifier type to String
        /// </summary>
        private string ConvertSidToString(SecurityIdentifier sid)
        {
            string returnSid = null;

            if (sid != null && sid.BinaryLength > 0)
            {
                var byteSid = new byte[sid.BinaryLength];
                sid.GetBinaryForm(byteSid, 0);
                returnSid = string.Join(string.Empty, byteSid);
            }

            return returnSid;
        }

        #endregion

        public void Dispose()
        {
            if (_principalContexts != null)
                _principalContexts.AsParallel().ForAll(pc => pc.Dispose());
        }
    }
}