using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Alcoa.Entity.Repository
{
    /// <summary>
    /// Base repository implementation to be used in Unit Of Work
    /// </summary>
    public class BaseRepository<TType> : IRepository<TType>
        where TType : Base, IBaseDomain
    {
        protected readonly IRepository<TType> _dbRepo;

        public BaseRepository(IRepository<TType> repo)
        {
            _dbRepo = repo;
        }

        public virtual TType Insert(TType entity)
        {
            return _dbRepo.Insert(entity);
        }

        public virtual bool Update(TType entity)
        {
            return _dbRepo.Update(entity);
        }

        public virtual bool Delete(TType entity)
        {
            return _dbRepo.Delete(entity);
        }

        public virtual TType Get(Expression<Func<TType, bool>> whereCondition = null, params string[] includes)
        {
            return _dbRepo.Get(whereCondition, includes);
        }

        public virtual IQueryable<TType> SelectWhere(Expression<Func<TType, bool>> whereCondition = null, params string[] includes)
        {
            return _dbRepo.SelectWhere(whereCondition, includes);
        }

        public void Dispose()
        {
            _dbRepo.Dispose();
        }
    }
}