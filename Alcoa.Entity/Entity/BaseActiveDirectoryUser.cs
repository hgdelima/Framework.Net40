using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Entity.Entity
{
    /// <summary>
    /// Base class with basic active directory user data that can be reused by many objects
    /// </summary>
    [DataContract(Namespace = "Alcoa.Entity")]
    public class BaseActiveDirectoryUser : BaseUser
    {
        public BaseActiveDirectoryUser()
        {
            ActiveDirectoryGroups = new List<BaseActiveDirectoryGroup>();
            ActiveDirectoryThirdPartners = new List<BaseUser>();
            ActiveDirectoryManager = new BaseUser();
        }

        public BaseActiveDirectoryUser(
            string employeeId,
            string userSid,
            string name,
            string samAccountName,
            string emailAddress,
            string displayName,
            DateTime? accountExpirationDate,
            string givenName,
            string middleName,
            string surname,
            string voiceTelephoneNumber,
            bool? isEnabled)
        {
            Id = userSid ?? employeeId;
            Sid = userSid ?? employeeId;
            Domain = name;
            Login = samAccountName;
            Email = emailAddress;
            NameOrDescription = displayName;
            LoginExpirationDate = accountExpirationDate;
            UserExtraInfo = new BaseUserExtraInfo
            {
                FirstName = givenName,
                MiddleName = middleName,
                LastName = surname,
                Phone = voiceTelephoneNumber,
                IsEnabled = isEnabled,
            };

            ActiveDirectoryGroups = new List<BaseActiveDirectoryGroup>();
            ActiveDirectoryThirdPartners = new List<BaseUser>();
            ActiveDirectoryManager = new BaseUser();
        }

        [DataMember]
        public List<BaseActiveDirectoryGroup> ActiveDirectoryGroups { get; set; }

        [DataMember]
        public List<BaseUser> ActiveDirectoryThirdPartners { get; set; }

        [DataMember]
        public BaseUser ActiveDirectoryManager { get; set; }
    }
}