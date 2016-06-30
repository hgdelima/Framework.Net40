using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;

namespace Alcoa.Entity.Entity
{
    /// <summary>
    /// Base class with e-mail attachment data that can be reused by many objects
    /// </summary>
    [DataContract(Namespace = "Alcoa.Entity")]
    public class BaseEmailAttachment : Base
    {
        [DataMember]
        public string FileNameWithExtension { get; set; }

        [DataMember]
        public ContentType ContentType { get; set; }

        [DataMember]
        public Stream FileStream { get; set; }
    }
}