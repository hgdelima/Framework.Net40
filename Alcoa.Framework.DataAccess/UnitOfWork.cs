using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Entity.Repository;
using Alcoa.Framework.Connection;
using Alcoa.Framework.Connection.Factory;
using System;
using System.Collections;
using System.Data;
using System.Data.Entity;

namespace Alcoa.Framework.DataAccess
{
    public class UnitOfWork<T> : IUnitOfWork
        where T : DbContext
    {
        private BaseUnitOfWork<T> _baseUow { get; set; }

        public UnitOfWork()
        {
            _baseUow = new BaseUnitOfWork<T>();
        }

        public UnitOfWork(string profileName)
        {
            _baseUow = new BaseUnitOfWork<T>(profileName);
        }

        public BaseRepository<R> GetRepository<R>()
            where R : Base, IBaseDomain
        {
            return new BaseRepository<R>(_baseUow.GetRepository<R>()); ;
        }

        public IActiveDirectoryRepository GetRepositoryActiveDirectory()
        {
            return new ActiveDirectoryFactory().CreateInstance();
        }

        public void Commit()
        {
            _baseUow.Commit();
        }

        public void Rollback()
        {
            _baseUow.Rollback();
        }

        public int ExecuteNonQuery(string sqlCommand, Hashtable parameters = null, CommandType commandType = CommandType.Text)
        {
            return _baseUow.ExecuteNonQuery(sqlCommand, parameters, commandType);
        }

        public DataSet ExecuteDataset(string sqlCommand, Hashtable parameters = null, CommandType commandType = CommandType.Text)
        {
            return _baseUow.ExecuteDataset(sqlCommand, parameters, commandType);
        }

        public Ty ExecuteScalar<Ty>(string sqlCommand, Hashtable parameters = null, CommandType commandType = CommandType.Text)
        {
            return _baseUow.ExecuteScalar<Ty>(sqlCommand, parameters, commandType);
        }

        public void Dispose()
        {
            _baseUow.Dispose();
        }
    }
}