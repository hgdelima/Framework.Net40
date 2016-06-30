using Alcoa.Entity.Entity;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Sso
{
    [DataContract(Namespace = "Alcoa.Sso")]
    public class SsoAuthorizationDTO
    {
        [DataMember]
        public bool IsValid { get; set; }

        [DataMember]
        public List<BaseIdentification> Claims { get; set; }
    }
}