using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Sso
{
    [DataContract(Namespace = "Alcoa.Sso")]
    public class SsoAuthenticationDTO
    {
        [DataMember]
        public string EncriptedAppCode { get; set; }

        [DataMember]
        public string EncriptedLogin { get; set; }

        [DataMember]
        public string EncriptedPassword { get; set; }

        [DataMember]
        public string LanguageCultureName { get; set; }
    }
}