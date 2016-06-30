using Alcoa.Entity.Entity;
using Alcoa.Framework.Contract.DTOs.Sso;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class ProfileDTO : BaseIdentification
    {
        public ProfileDTO()
        {
            ActiveDirectoryGroups = new List<ActiveDirectoryGroupDTO>();
            SsoGroups = new List<SsoGroupDTO>();
            Users = new List<SsoUserDTO>();
        }

        [DataMember]
        public string ProfileType { get; set; }

        [DataMember]
        public string IsPublic { get; set; }

        [DataMember]
        public string Order { get; set; }

        [DataMember]
        public List<ActiveDirectoryGroupDTO> ActiveDirectoryGroups { get; set; }

        [DataMember]
        public List<SsoGroupDTO> SsoGroups { get; set; }

        [DataMember]
        public List<SsoUserDTO> Users { get; set; }

        [DataMember]
        public string SourceDatabase { get; set; }
    }
}