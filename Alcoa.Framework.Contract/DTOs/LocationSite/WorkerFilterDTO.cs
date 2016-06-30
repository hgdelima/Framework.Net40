using Alcoa.Entity.Entity;
using Alcoa.Framework.Common.Enumerator;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.LocationSite
{
    [DataContract(Namespace = "Alcoa.LocationSite")]
    public class WorkerFilterDTO : BaseIdentification
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public CommonDatabase SpecificDatabase { get; set; }
    }
}