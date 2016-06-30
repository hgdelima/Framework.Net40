using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class BudgetCodeFilterDTO
    {
        [DataMember]
        public string SiteId { get; set; }

        [DataMember]
        public string AreaId { get; set; }

        [DataMember]
        public string DeptId { get; set; }

        [DataMember]
        public string LbcId { get; set; }
    }
}