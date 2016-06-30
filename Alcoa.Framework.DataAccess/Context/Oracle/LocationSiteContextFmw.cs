using Alcoa.Framework.Domain.Entity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Alcoa.Framework.DataAccess.Context.Oracle
{
    public class LocationSiteContextFmw : DbContext
    {
        public LocationSiteContextFmw(string profileName)
            : base(profileName)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<NavigationPropertyNameForeignKeyDiscoveryConvention>();

            modelBuilder.Configurations.Add(new LocationSiteConfigurations.WorkerConfiguration());
            modelBuilder.Configurations.Add(new LocationSiteConfigurations.BudgetCodeConfiguration());
            modelBuilder.Configurations.Add(new LocationSiteConfigurations.AreaConfiguration());
            modelBuilder.Configurations.Add(new LocationSiteConfigurations.SiteConfiguration());
            modelBuilder.Configurations.Add(new LocationSiteConfigurations.LbcConfiguration());
            modelBuilder.Configurations.Add(new LocationSiteConfigurations.DeptConfiguration());
            modelBuilder.Configurations.Add(new LocationSiteConfigurations.ApplicationConfiguration());
            modelBuilder.Configurations.Add(new LocationSiteConfigurations.ProfileConfiguration());
            modelBuilder.Configurations.Add(new LocationSiteConfigurations.ProfileAndWorkerConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}