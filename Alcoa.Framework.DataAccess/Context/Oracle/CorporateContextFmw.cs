using Alcoa.Framework.Domain.Entity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Alcoa.Framework.DataAccess.Context.Oracle
{
    public class CorporateContextFmw : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<NavigationPropertyNameForeignKeyDiscoveryConvention>();

            modelBuilder.Configurations.Add(new CorporateConfigurations.WorkerConfiguration());
            modelBuilder.Configurations.Add(new CorporateConfigurations.WorkerHierarchyConfiguration());
            modelBuilder.Configurations.Add(new CorporateConfigurations.AreaConfiguration());
            modelBuilder.Configurations.Add(new CorporateConfigurations.SiteConfiguration());
            modelBuilder.Configurations.Add(new CorporateConfigurations.LbcConfiguration());
            modelBuilder.Configurations.Add(new CorporateConfigurations.DeptConfiguration());
            modelBuilder.Configurations.Add(new CorporateConfigurations.BudgetCodeConfiguration());
            modelBuilder.Configurations.Add(new CorporateConfigurations.ApplicationConfiguration());
            modelBuilder.Configurations.Add(new CorporateConfigurations.ProfileConfiguration());
            modelBuilder.Configurations.Add(new CorporateConfigurations.ProfileAndWorkerConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}