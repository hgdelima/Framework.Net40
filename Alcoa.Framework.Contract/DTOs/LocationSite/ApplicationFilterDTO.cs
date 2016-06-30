using Alcoa.Framework.Common.Enumerator;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.LocationSite
{
    [DataContract(Namespace = "Alcoa.LocationSite")]
    public class ApplicationFilterDTO
    {
        [DataMember]
        public string ApplicationCode { get; set; }

        [DataMember]
        public bool LoadUsersProfiles { get; set; }

        [DataMember]
        public CommonDatabase SpecificDatabase { get; set; }
    }
}