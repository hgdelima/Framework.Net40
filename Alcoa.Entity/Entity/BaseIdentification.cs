using System.Runtime.Serialization;

namespace Alcoa.Entity.Entity
{
    /// <summary>
    /// Base class with identification data that can be reused by many objects
    /// </summary>
    [DataContract(Namespace = "Alcoa.Entity")]
    public class BaseIdentification : Base
    {
        /// <summary>
        /// Base identification constructor
        /// </summary>
        public BaseIdentification()
        { }

        /// <summary>
        /// Base identification constructor passing Id and Name, Value or Description
        /// </summary>
        public BaseIdentification(string id, string nameOrDescription)
        {
            Id = id;
            NameOrDescription = nameOrDescription;
        }

        /// <summary>
        /// Id to be used as key property
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Name, Value or Description to be used for many purposes
        /// </summary>
        [DataMember]
        public string NameOrDescription { get; set; }
    }
}