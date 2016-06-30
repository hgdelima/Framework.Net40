
using System.Runtime.Serialization;
namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class SiteFilterDTO
    {
        [DataMember]
        public string Id { get; set; }
    }
}