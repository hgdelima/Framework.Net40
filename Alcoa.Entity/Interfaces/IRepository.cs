using System;
using System.Linq;
using System.Linq.Expressions;

namespace Alcoa.Entity.Interfaces
{
    /// <summary>
    /// Defines an interface for common Repository operations, like Databases or Files.
    /// </summary>
    public interface IRepository<TType> : ICrud<TType>, IDisposable
         where TType : class, IBaseDomain
    {
        /// <summary>
        /// Select registers from a Repository filtering using WhereCondition parameter
        /// </summary>
        IQueryable<TType> SelectWhere(Expression<Func<TType, bool>> whereCondition = null, params string[] includes);
    }
}