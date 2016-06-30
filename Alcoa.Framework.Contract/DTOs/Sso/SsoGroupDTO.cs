using Alcoa.Entity.Entity;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Sso
{
    [DataContract(Namespace = "Alcoa.Sso")]
    public class SsoGroupDTO : BaseIdentification
    {
        public SsoGroupDTO()
        {
            Services = new List<SsoServicesDTO>();
        }

        [DataMember]
        public List<SsoServicesDTO> Services { get; set; }
    }
}