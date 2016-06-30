using Alcoa.Entity.Entity;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class LbcDTO : BaseLbc
    {
        [DataMember]
        public string LbcId { get; set; }
    }
}