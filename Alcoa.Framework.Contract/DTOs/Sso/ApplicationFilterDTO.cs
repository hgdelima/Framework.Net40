using Alcoa.Entity.Entity;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Sso
{
    [DataContract(Namespace = "Alcoa.Sso")]
    public class ApplicationFilterDTO
    {
        [DataMember]
        public string ApplicationCode { get; set; }

        [DataMember]
        public string LanguageCultureName { get; set; }

        [DataMember]
        public bool LoadTranslations { get; set; }

        [DataMember]
        public bool LoadUsersProfiles { get; set; }

        [DataMember]
        public bool LoadActiveDirectoryExtraInfo { get; set; }
    }
}