using Alcoa.Entity.Entity;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class DeptDTO : BaseMultiLanguageDescription
    {
        [DataMember]
        public string DeptId { get; set; }
    }
}