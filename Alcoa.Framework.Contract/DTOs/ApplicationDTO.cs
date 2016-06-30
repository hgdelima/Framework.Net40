using Alcoa.Entity.Entity;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class ApplicationDTO : BaseIdentification
    {
        public ApplicationDTO()
        {
            Profiles = new List<ProfileDTO>();
        }

        [DataMember]
        public string HomeUrl { get; set; }

        [DataMember]
        public string Mnemonic { get; set; }

        [DataMember]
        public string IsInactive { get; set; }

        [DataMember]
        public string ToolType { get; set; }

        [DataMember]
        public List<ProfileDTO> Profiles { get; set; }
    }
}