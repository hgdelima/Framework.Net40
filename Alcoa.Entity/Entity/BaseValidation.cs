using Alcoa.Entity.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Alcoa.Entity.Entity
{
    /// <summary>
    /// Base class that implements IBaseValidation interface to handle objects validations
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "Alcoa.Entity")]
    public class BaseValidation : IBaseValidation
    {
        /// <summary>
        /// Initializes object validations with default values
        /// </summary>
        public BaseValidation()
        {
            ForceCommitWhenErrors = false;
            Results = new List<ValidationResult>();
        }

        /// <summary>
        /// Indicate if object has errors
        /// </summary>
        [DataMember]
        public bool HasErrors
        {
            get { return (Results.Count > 0); }
            set { }
        }

        /// <summary>
        /// Option to force commits at database even if the object has errors
        /// </summary>
        [XmlIgnore]
        [IgnoreDataMember]
        public bool ForceCommitWhenErrors { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore] 
        [IgnoreDataMember]
        public List<ValidationResult> Results { get; set; }
    }
}