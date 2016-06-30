using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class BudgetCodeListFilterDTO
    {
        [DataMember]
        public string SiteId { get; set; }

        [DataMember]
        public string AreaId { get; set; }

        [DataMember]
        public string DeptId { get; set; }

        [DataMember]
        public string LbcId { get; set; }

        [DataMember]
        public string IsActive { get; set; }

        [DataMember]
        public bool LoadSite { get; set; }

        [DataMember]
        public bool LoadArea { get; set; }

        [DataMember]
        public bool LoadDept { get; set; }

        [DataMember]
        public bool LoadLbc { get; set; }

        [DataMember]
        public bool LoadManager { get; set; }
    }
}