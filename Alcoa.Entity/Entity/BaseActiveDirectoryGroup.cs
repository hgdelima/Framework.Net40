using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Entity.Entity
{
    /// <summary>
    /// Base class with basic active directory group data that can be reused by many objects
    /// </summary>
    [DataContract(Namespace = "Alcoa.Entity")]
    public class BaseActiveDirectoryGroup : BaseIdentification
    {
        public BaseActiveDirectoryGroup()
        {
            Users = new List<BaseIdentification>();
        }

        [DataMember]
        public string Owner { get; set; }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public DateTime? CreationDate { get; set; }

        [DataMember]
        public List<BaseIdentification> Users { get; set; }
    }
}