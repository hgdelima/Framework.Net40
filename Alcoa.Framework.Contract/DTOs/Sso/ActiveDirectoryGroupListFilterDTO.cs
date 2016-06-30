using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Sso
{
    [DataContract(Namespace = "Alcoa.Sso")]
    public class ActiveDirectoryGroupListFilterDTO
    {
        [DataMember]
        public List<string> GroupIdentities { get; set; }

        [DataMember]
        public string Domain { get; set; }
    }
}