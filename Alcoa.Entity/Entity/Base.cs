using Alcoa.Entity.Interfaces;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Alcoa.Entity.Entity
{
    /// <summary>
    /// Base class that implements IBase interface and can be reused by many objects
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "Alcoa.Entity")]
    public class Base : IBase
    {
        /// <summary>
        /// Initializes base entity with unique id (guid)
        /// </summary>
        public Base()
        {
            UniqueId = Guid.NewGuid();
        }

        /// <summary>
        /// Unique Id that identifies object instances
        /// </summary>
        [XmlIgnore]
        [IgnoreDataMember]
        public Guid UniqueId { get; set; }
    }
}