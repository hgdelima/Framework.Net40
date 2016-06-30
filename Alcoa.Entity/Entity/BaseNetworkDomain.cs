using System.Runtime.Serialization;

namespace Alcoa.Entity.Entity
{
    /// <summary>
    /// Base class with active directory data that can be reused by many objects
    /// </summary>
    [DataContract(Namespace = "Alcoa.Entity")]
    public class BaseNetworkDomain : Base
    {
        [DataMember]
        public string DomainName { get; set; }

        [DataMember]
        public string Container { get; set; }
    }
}