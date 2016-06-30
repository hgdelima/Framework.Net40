using System.Runtime.Serialization;

namespace Alcoa.Framework.Contract.DTOs.Corporate
{
    [DataContract(Namespace = "Alcoa.Corporate")]
    public class ConnectionStringFilterDTO
    {
        [DataMember]
        public string ConnectionString { get; set; }
    }
}