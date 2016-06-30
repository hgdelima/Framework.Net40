using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Alcoa.Framework.UnitTest.DataAccess
{
    [TestClass]
    public class SsoProfileAndServiceRepositoryTest
    {
        private IUnitOfWork uow;
        private IRepository<SsoProfileAndService> profilesServicesRepo;

        [TestInitialize]
        public void Initialize()
        {
            uow = new UnitOfWork<SsoContextSso>();
            profilesServicesRepo = uow.GetRepository<SsoProfileAndService>();
        }

        [TestMethod]
        public void GetPublicSsoServicesList()
        {
            var profilesAndServices = profilesServicesRepo
                .SelectWhere(ps => ps.Service.IsPublic == "S", "Profile", "Service")
                .ToList();

            Assert.IsNotNull(profilesAndServices);
            Assert.IsTrue(profilesAndServices.Any());
        }

        [TestMethod]
        public void GetSsoServicesAndSsoGroupHierarchyList()
        {
            var profilesServicesGroups = profilesServicesRepo
                .SelectWhere(ps => ps.ApplicationId == "SHPF",
                "Service",
                "Service.SsoGroup",
                "Service.SsoGroup.SubSsoGroups")
                .ToList();

            Assert.IsNotNull(profilesServicesGroups);
            Assert.IsTrue(profilesServicesGroups.Any());
        }
    }
}
