using Alcoa.Entity.Entity;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class AreaLeafDTO : BaseDescription
    {
        [DataMember]
        public string AreaId { get; set; }

        [DataMember]
        public string AreaParentId { get; set; }

        [DataMember]
        public string SiteId { get; set; }

        [DataMember]
        public string SiteParentId { get; set; }

        [DataMember]
        public WorkerLeafDTO Manager { get; set; }
    }
}