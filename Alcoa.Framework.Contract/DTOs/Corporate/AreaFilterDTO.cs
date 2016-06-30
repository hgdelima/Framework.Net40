using Alcoa.Entity.Entity;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class AreaFilterDTO
    {
        [DataMember]
        public string SiteId { get; set; }

        [DataMember]
        public string AreaId { get; set; }
    }
}