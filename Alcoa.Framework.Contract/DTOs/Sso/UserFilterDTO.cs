using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Sso
{
    [DataContract(Namespace = "Alcoa.Sso")]
    public class UserFilterDTO
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string LanguageCultureName { get; set; }

        [DataMember]
        public bool LoadProfiles { get; set; }

        [DataMember]
        public bool LoadPublicServices { get; set; }

        [DataMember]
        public bool LoadTranslations { get; set; }
    }
}