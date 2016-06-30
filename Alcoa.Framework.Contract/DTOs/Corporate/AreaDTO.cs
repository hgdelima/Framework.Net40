using Alcoa.Entity.Entity;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class AreaDTO : BaseDescription
    {
        [DataMember]
        public string AreaId { get; set; }

        [DataMember]
        public AreaLeafDTO AreaParent { get; set; }

        [DataMember]
        public List<AreaLeafDTO> SubAreas { get; set; }

        [DataMember]
        public SiteLeafDTO Site { get; set; }

        [DataMember]
        public string SiteParentId { get; set; }

        [DataMember]
        public List<BudgetCodeDTO> BudgetCodes { get; set; }

        [DataMember]
        public WorkerLeafDTO Manager { get; set; }
    }
}