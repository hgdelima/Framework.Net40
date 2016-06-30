using Alcoa.Entity.Entity;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Sso
{
    [DataContract(Namespace = "Alcoa.Sso")]
    public class SsoServicesDTO : BaseIdentification
    {
        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public string IsFullScreen { get; set; }
    }
}