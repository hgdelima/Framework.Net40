using Alcoa.Entity.Entity;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class AreaListFilterDTO : BaseDescription
    {
        [DataMember]
        public bool LoadAreaParent { get; set; }

        [DataMember]
        public bool LoadSubAreas { get; set; }

        [DataMember]
        public bool LoadSite { get; set; }

        [DataMember]
        public bool LoadBudgetCodes { get; set; }

        [DataMember]
        public bool LoadManager { get; set; }
    }
}