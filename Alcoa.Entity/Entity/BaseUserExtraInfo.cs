using System;
using System.Runtime.Serialization;

namespace Alcoa.Entity.Entity
{
    public enum AccountType
    {
        DomainUser,
        DomainSharedUser,
        SSOUser
    }

    /// <summary>
    /// Base class with user extra data that can be reused by many objects
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "Alcoa.Entity")]
    public class BaseUserExtraInfo
    {
        /// <summary>
        /// Initializes as Invalid User, and AD or Database fill the right value
        /// </summary>
        public BaseUserExtraInfo()
        {
            AccountTypeName = AccountType.DomainUser.ToString();
        }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Office { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string AccountTypeName { get; set; }

        [DataMember]
        public string IP { get; set; }

        [DataMember]
        public bool? IsLocked { get; set; }

        [DataMember]
        public bool? IsEnabled { get; set; }
    }
}