using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Sso
{
    [DataContract(Namespace = "Alcoa.Sso")]
    public class UserListFilterDTO
    {
        [DataMember]
        public List<string> LoginsList { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Domain { get; set; }
    }
}