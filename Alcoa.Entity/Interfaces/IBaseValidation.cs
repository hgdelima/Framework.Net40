using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alcoa.Entity.Interfaces
{
    /// <summary>
    /// Interface for validation object in domain objects
    /// </summary>
    public interface IBaseValidation
    {
        /// <summary>
        /// Return true if Result list has any itens
        /// </summary>
        bool HasErrors { get; }

        /// <summary>
        /// Setting to true will ignore HasErrors verification when executing Insert, Update and Delete repository commands
        /// </summary>
        bool ForceCommitWhenErrors { get; set; }

        /// <summary>
        /// Handles validation results executed by objects
        /// </summary>
        List<ValidationResult> Results { get; set; }
    }
}