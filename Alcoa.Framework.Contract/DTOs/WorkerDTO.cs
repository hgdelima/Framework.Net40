using Alcoa.Entity.Entity;
using Alcoa.Framework.Contract.DTOs.Corporate;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class WorkerDTO : BaseEmployee
    {
        [DataMember]
        public WorkerLeafDTO Manager { get; set; }

        [DataMember]
        public List<WorkerLeafDTO> Employees { get; set; }

        [DataMember]
        public List<ThirdPartnerDTO> ThirdPartners { get; set; }

        [DataMember]
        public BudgetCodeDTO BudgetCode { get; set; }

        [DataMember]
        public string SourceDatabase { get; set; }
    }
}