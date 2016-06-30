using Alcoa.Framework.Domain.Entity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Alcoa.Framework.DataAccess.Context.Oracle
{
    public class EmailContextSsm : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<NavigationPropertyNameForeignKeyDiscoveryConvention>();

            modelBuilder.Configurations.Add(new EmailConfigurations.EmailGatewayConfiguration());
            modelBuilder.Configurations.Add(new EmailConfigurations.EmailGatewayLogConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}