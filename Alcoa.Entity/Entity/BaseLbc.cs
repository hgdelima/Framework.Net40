using System.Runtime.Serialization;

namespace Alcoa.Entity.Entity
{
    /// <summary>
    /// Base class with basic LBC data that can be reused by many objects
    /// </summary>
    [DataContract(Namespace = "Alcoa.Entity")]
    public class BaseLbc : BaseMultiLanguageDescription
    {
        [DataMember]
        public string PrefixId { get; set; }

        [DataMember]
        public string IsHeadQuarter { get; set; }

        [DataMember]
        public int CityId { get; set; }
    }
}