using Alcoa.Entity.Entity;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class SiteDTO : BaseIdentification
    {
        [DataMember]
        public string IsActive { get; set; }

        [DataMember]
        public List<LbcDTO> Lbcs { get; set; }

        [DataMember]
        public List<AreaDTO> Areas { get; set; }
    }
}