using Alcoa.Framework.Domain.Entity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Alcoa.Framework.DataAccess.Context.Oracle
{
    public class SsoContextSso : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<NavigationPropertyNameForeignKeyDiscoveryConvention>();

            modelBuilder.Configurations.Add(new SsoConfigurations.WorkerConfiguration());
            modelBuilder.Configurations.Add(new SsoConfigurations.ApplicationParameterConfiguration());
            modelBuilder.Configurations.Add(new SsoConfigurations.ApplicationTranslationConfiguration());

            modelBuilder.Configurations.Add(new SsoConfigurations.SsoApplicationConfiguration());
            modelBuilder.Configurations.Add(new SsoConfigurations.SsoProfileConfiguration());
            modelBuilder.Configurations.Add(new SsoConfigurations.SsoProfileAndServiceConfiguration());
            modelBuilder.Configurations.Add(new SsoConfigurations.SsoProfileAndActiveDirectoryConfiguration());
            modelBuilder.Configurations.Add(new SsoConfigurations.SsoProfileAndWorkerConfiguration());
            modelBuilder.Configurations.Add(new SsoConfigurations.SsoServiceConfiguration());
            modelBuilder.Configurations.Add(new SsoConfigurations.SsoGroupConfiguration());
            modelBuilder.Configurations.Add(new SsoConfigurations.ActiveDirectoryGroupConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}