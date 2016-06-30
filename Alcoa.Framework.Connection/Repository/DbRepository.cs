using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Alcoa.Framework.Connection.Repository
{
    /// <summary>
    /// Defines a class for generic entity repository to access common functionality for data access layer.
    /// </summary>
    /// <remarks>Can't be debugged</remarks>
    [Browsable(false), DebuggerStepThrough]
    internal class DbRepository<TType> : IRepository<TType>
        where TType : Base, IBaseDomain
    {
        //Don´t allow developer see private context and dbset with connectionstring
        [Browsable(false), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DbContext _privateContext_;

        [Browsable(false), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DbSet<TType> _dbSet_;

        [Browsable(false), DebuggerHidden]
        public DbRepository(DbContext context)
        {
            _privateContext_ = context;
            _dbSet_ = _privateContext_.Set<TType>();
        }

        public virtual TType Insert(TType entity)
        {
            if (entity.Validation.ForceCommitWhenErrors || !entity.Validation.HasErrors)
                _dbSet_.Add(entity);

            return entity;
        }

        public virtual bool Update(TType entity)
        {
            if (entity.Validation.ForceCommitWhenErrors || !entity.Validation.HasErrors)
            {
                _dbSet_.Attach(entity);

                var entry = _privateContext_.ChangeTracker.Entries<TType>().FirstOrDefault(e => e.Entity == entity);
                if (entry != null)
                    entry.State = EntityState.Modified;

                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool Delete(TType entity)
        {
            if (entity.Validation.ForceCommitWhenErrors || !entity.Validation.HasErrors)
            {
                _dbSet_.Remove(entity);
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual TType Get(Expression<Func<TType, bool>> whereCondition = null, params string[] includes)
        {
            IQueryable<TType> dbQuery = _dbSet_;
            dbQuery = includes.Aggregate(dbQuery, (current, include) => current.Include(include));

            var objQuery = (whereCondition == null) ? dbQuery.AsQueryable() : dbQuery.Where(whereCondition);

            return (objQuery.FirstOrDefault() ?? Activator.CreateInstance<TType>());
        }

        public virtual IQueryable<TType> SelectWhere(Expression<Func<TType, bool>> whereCondition = null, params string[] includes)
        {
            IQueryable<TType> dbQuery = _dbSet_;
            dbQuery = includes.Aggregate(dbQuery, (current, include) => current.Include(include));

            return (whereCondition == null) ? dbQuery.AsQueryable() : dbQuery.Where(whereCondition);
        }

        public void Dispose()
        {
            _dbSet_ = null;
        }
    }
}