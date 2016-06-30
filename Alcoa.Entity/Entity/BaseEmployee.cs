using System;
using System.Runtime.Serialization;

namespace Alcoa.Entity.Entity
{
    /// <summary>
    /// Base class with basic employee (worker) data that can be reused by many objects
    /// </summary>
    [DataContract(Namespace = "Alcoa.Entity")]
    public class BaseEmployee : BaseUser
    {
        [DataMember]
        public string FixedId { get; set; }

        [DataMember]
        public string AssociationType { get; set; }

        [DataMember]
        public string BranchLine { get; set; }

        [DataMember]
        public string Nickname { get; set; }

        [DataMember]
        public string Gender { get; set; }

        [DataMember]
        public string WebSignature { get; set; }

        [DataMember]
        public string Local { get; set; }

        [DataMember]
        public string PhysicalLocal { get; set; }

        [DataMember]
        public string IdentificationDocument { get; set; }

        [DataMember]
        public string DeptId { get; set; }

        [DataMember]
        public string LbcId { get; set; }

        [DataMember]
        public string FiscalId { get; set; }

        [DataMember]
        public string AccountingEntityId { get; set; }

        [DataMember]
        public string BusinessTitle { get; set; }

        [DataMember]
        public string ClassificationName { get; set; }

        [DataMember]
        public DateTime? ActualExchangeDate { get; set; }

        [DataMember]
        public DateTime? BirthDate { get; set; }
    }
}