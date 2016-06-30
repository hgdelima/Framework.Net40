using Alcoa.Entity.Entity;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class WorkerFilterDTO : BaseIdentification
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public bool LoadEmployees { get; set; }

        [DataMember]
        public bool LoadManager { get; set; }

        [DataMember]
        public bool LoadThirdPartners { get; set; }
    }
}