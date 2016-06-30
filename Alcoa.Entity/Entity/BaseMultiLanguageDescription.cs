using System.Runtime.Serialization;

namespace Alcoa.Entity.Entity
{
    /// <summary>
    /// Base class with multi language description data that can be reused by many objects
    /// </summary>
    [DataContract(Namespace = "Alcoa.Entity")]
    public class BaseMultiLanguageDescription : Base
    {
        [DataMember]
        public string EspanishDescription { get; set; }

        [DataMember]
        public string PortugueseDescription { get; set; }

        [DataMember]
        public string EnglishDescription { get; set; }

        [DataMember]
        public string IsActive { get; set; }

        [DataMember]
        public string IsActiveGl { get; set; }
    }
}