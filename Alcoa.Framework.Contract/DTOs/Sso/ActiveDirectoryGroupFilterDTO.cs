using Alcoa.Entity.Entity;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Sso
{
    [DataContract(Namespace = "Alcoa.Sso")]
    public class ActiveDirectoryGroupFilterDTO
    {
        [DataMember]
        public string GroupIdentity { get; set; }
    }
}