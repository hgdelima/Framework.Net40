using System.Data.Common;
using System.Data.Entity;
using Alcoa.Framework.Domain.Entity;

namespace Alcoa.Framework.DataAccess.Context.SQLServer
{
    public class TestContextRfc : DbContext
    {
        public TestContextRfc(DbConnection dbConn)
            : base(dbConn, true)
        {
            Database.SetInitializer<TestContextRfc>(null);
        }

        public DbSet<Worker> Workers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TestConfigurations.WorkerConfiguration());
        }
    }
}