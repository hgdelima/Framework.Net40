using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Sso
{
    [DataContract(Namespace = "Alcoa.Sso")]
    public class ApplicationListFilterDTO
    {
        [DataMember]
        public string LanguageCultureName { get; set; }
    }
}