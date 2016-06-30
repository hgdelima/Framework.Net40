using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class WorkerListFilterDTO
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public List<string> Logins { get; set; }

        [DataMember]
        public List<string> Ids { get; set; }

        [DataMember]
        public bool LoadEmployees { get; set; }

        [DataMember]
        public bool LoadManager { get; set; }

        [DataMember]
        public bool LoadBudgetCode { get; set; }

        [DataMember]
        public bool LoadThirdPartners { get; set; }
    }
}