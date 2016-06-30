using Alcoa.Entity.Entity;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class BudgetCodeDTO : Base
    {
        [DataMember]
        public SiteLeafDTO Site { get; set; }

        [DataMember]
        public AreaLeafDTO Area { get; set; }

        [DataMember]
        public DeptDTO Dept { get; set; }

        [DataMember]
        public LbcDTO Lbc { get; set; }

        [DataMember]
        public string IsActive { get; set; }

        [DataMember]
        public WorkerLeafDTO Manager { get; set; }
    }
}