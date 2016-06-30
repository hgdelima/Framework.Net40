using System;
using System.Collections;
using System.Data;

namespace Alcoa.Entity.Interfaces
{
    /// <summary>
    /// Interface for Unit Of Work pattern to entity framework transactions
    /// </summary>
    public interface IBaseUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commit all changes to Repositories
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback tracked changes in all Repositories
        /// </summary>
        void Rollback();

        /// <summary>
        /// Executes a DbCommand that returns only affected rows
        /// </summary>
        int ExecuteNonQuery(string sqlCommand, Hashtable parameters = null, CommandType commandType = CommandType.Text);

        /// <summary>
        /// Executes a DbCommand that returns a resultset using the provided parameters.
        /// </summary>
        DataSet ExecuteDataset(string sqlCommand, Hashtable parameters = null, CommandType commandType = CommandType.Text);

        /// <summary>
        /// Executes a DbCommand that returns the first collumn from the first row as an object
        /// </summary>
        Ty ExecuteScalar<Ty>(string sqlCommand, Hashtable parameters = null, CommandType commandType = CommandType.Text);
    }
}