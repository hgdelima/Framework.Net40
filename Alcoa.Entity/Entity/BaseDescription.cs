
using System.Runtime.Serialization;
namespace Alcoa.Entity.Entity
{
    /// <summary>
    /// Base class with basic description data that can be reused by many objects
    /// </summary>
    [DataContract(Namespace = "Alcoa.Entity")]
    public class BaseDescription : Base
    {
        [DataMember]
        public string NameOrDescription { get; set; }

        [DataMember]
        public string IsActive { get; set; }
    }
}