using Alcoa.Entity.Entity;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class UserDTO : BaseEmployee
    {
        [DataMember]
        public List<ApplicationDTO> Applications { get; set; }
    }
}