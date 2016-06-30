using Alcoa.Framework.Common.Enumerator;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.LocationSite
{
    [DataContract(Namespace = "Alcoa.LocationSite")]
    public class WorkerListFilterDTO
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public List<string> Logins { get; set; }

        [DataMember]
        public List<string> Ids { get; set; }

        [DataMember]
        public bool LoadBudgetCode { get; set; }

        [DataMember]
        public CommonDatabase SpecificDatabase { get; set; } 
    }
}