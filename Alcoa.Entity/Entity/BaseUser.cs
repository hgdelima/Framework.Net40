using System;
using System.Runtime.Serialization;

namespace Alcoa.Entity.Entity
{
    /// <summary>
    /// Base class with basic user data that can be reused by many objects
    /// </summary>
    [DataContract(Namespace = "Alcoa.Entity")]
    public class BaseUser : BaseIdentification
    {
        public BaseUser()
        {
            UserExtraInfo = new BaseUserExtraInfo();
        }

        [DataMember]
        public string Sid { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Domain { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public DateTime? LoginExpirationDate { get; set; }

        [DataMember]
        public BaseUserExtraInfo UserExtraInfo { get; set; }
    }
}