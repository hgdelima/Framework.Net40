using System;

namespace Alcoa.Entity.Interfaces
{
    /// <summary>
    /// Interface to all domain and transfer objects in Alcoa´s system
    /// </summary>
    public interface IBase
    {
        /// <summary>
        /// Unique id property generated at run-time
        /// </summary>
        Guid UniqueId { get; set; }
    }
}