using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class ApplicationListFilterDTO
    {
        [DataMember]
        public bool IsActive { get; set; }
    }
}