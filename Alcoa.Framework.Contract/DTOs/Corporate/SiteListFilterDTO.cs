using Alcoa.Entity.Entity;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class SiteListFilterDTO : BaseDescription
    {
        [DataMember]
        public bool LoadLbcs { get; set; }

        [DataMember]
        public bool LoadAreas { get; set; }
    }
}