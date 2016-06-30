using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Common;
using Alcoa.Framework.Connection.Properties;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Alcoa.Framework.Connection.Repository
{
    /// <summary>
    /// Defines a class to persist objects as files
    /// </summary>
    internal class FileRepository<TType> : IRepository<TType>
         where TType : Base, IBaseDomain
    {
        private string _repositoryDirectory = string.Empty;

        public FileRepository()
        {
            _repositoryDirectory = AppDomain.CurrentDomain.BaseDirectory + Resources.FileRepositoryFolderName;

            if (!Directory.Exists(_repositoryDirectory))
                Directory.CreateDirectory(_repositoryDirectory);
        }

        public virtual TType Insert(TType entity) 
        {
            FileHelper.SaveAsFile(GetFullFileNameAndPath(entity), entity.Serialize());

            return entity;
        }

        public virtual bool Update(TType entity)
        {
            return FileHelper.SaveAsFile(GetFullFileNameAndPath(entity), entity.Serialize());
        }

        public virtual bool Delete(TType entity)
        {
            return FileHelper.DeleteFile(GetFullFileNameAndPath(entity));
        }

        public virtual TType Get(Expression<Func<TType, bool>> whereCondition = null, params string[] includes)
        {
            return FileHelper.OpenFiles(_repositoryDirectory)
                .Select(fc => fc.Deserialize<TType>())
                .FirstOrDefault();
        }

        public virtual IQueryable<TType> SelectWhere(Expression<Func<TType, bool>> whereCondition = null, params string[] includes)
        {
            return FileHelper.OpenFiles(_repositoryDirectory)
                .Select(fc => fc.Deserialize<TType>())
                .AsQueryable();
        }

        private string GetFullFileNameAndPath(TType entity)
        {
            var fileName = entity.GetType().Name + "_" + entity.UniqueId;

            return Path.Combine(_repositoryDirectory, fileName);
        }

        public void Dispose()
        {
            _repositoryDirectory = null;
        }
    }
}