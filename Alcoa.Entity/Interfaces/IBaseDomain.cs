using Alcoa.Entity.Entity;
using System.Xml.Serialization;

namespace Alcoa.Entity.Interfaces
{
    /// <summary>
    /// Interface for all domain objects
    /// </summary>
    public interface IBaseDomain
    {
        /// <summary>
        /// Handles validation results performed by objects
        /// </summary>
        BaseValidation Validation { get; set; }

        /// <summary>
        /// Performs basic object validations
        /// </summary>
        void Validate();
    }
}