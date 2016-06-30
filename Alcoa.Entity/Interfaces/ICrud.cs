using System;
using System.Linq.Expressions;

namespace Alcoa.Entity.Interfaces
{
    /// <summary>
    /// Interface for common system operations.
    /// </summary>
    public interface ICrud<TType>
        where TType : class
    {
        /// <summary>
        /// Saves an object at specific repository
        /// </summary>
        TType Insert(TType entity);

        /// <summary>
        /// Updates an object at specific repository
        /// </summary>
        bool Update(TType entity);

        /// <summary>
        /// Deletes an object at specific repository
        /// </summary>
        bool Delete(TType entity);

        /// <summary>
        /// Get just one object at specific repository using a specified condition
        /// </summary>
        TType Get(Expression<Func<TType, bool>> whereCondition = null, params string[] includes);
    }
}