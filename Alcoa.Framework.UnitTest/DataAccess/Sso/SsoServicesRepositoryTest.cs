using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Alcoa.Framework.UnitTest.DataAccess
{
    [TestClass]
    public class SsoServicesRepositoryTest
    {
        private IUnitOfWork uow;
        private IRepository<SsoServices> ssoServicesRepo;

        [TestInitialize]
        public void Initialize()
        {
            uow = new UnitOfWork<SsoContextSso>();
            ssoServicesRepo = uow.GetRepository<SsoServices>();
        }

        [TestMethod]
        public void GetSsoServices()
        {
            var ssoServices = ssoServicesRepo.Get(ss => 
                ss.ApplicationId == "RFC" && 
                ss.Id == "10");

            Assert.IsNotNull(ssoServices);
            Assert.IsNotNull(ssoServices.ApplicationId);
        }

        [TestMethod]
        public void GetSsoServicesList()
        {
            var ssoServices = ssoServicesRepo.SelectWhere(w => w.ApplicationId == "RFC");

            Assert.IsNotNull(ssoServices);
            Assert.IsTrue(ssoServices.Any());
        }

        [TestMethod]
        public void GetFullSsoServices()
        {
            var ssoServices = ssoServicesRepo.Get(ss =>
                ss.ApplicationId == "RFC" &&
                ss.Id == "10",
                "SsoGroup");

            Assert.IsNotNull(ssoServices);
            Assert.IsNotNull(ssoServices.ApplicationId);
        }
    }
}
