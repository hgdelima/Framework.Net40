using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Alcoa.Framework.UnitTest.DataAccess
{
    [TestClass]
    public class SsoApplicationRepositoryTest
    {
        private IUnitOfWork uow;
        private IRepository<SsoApplication> ssoApplicationRepo;

        [TestInitialize]
        public void Initialize()
        {
            uow = new UnitOfWork<SsoContextSso>();
            ssoApplicationRepo = uow.GetRepository<SsoApplication>();
        }

        [TestMethod]
        public void GetSsoApplication()
        {
            var ssoApp = ssoApplicationRepo.Get(sa => sa.Id == "RFC");

            Assert.IsNotNull(ssoApp);
            Assert.IsNotNull(ssoApp.Id);
        }

        [TestMethod]
        public void GetSsoApplicationList()
        {
            var ssoApps = ssoApplicationRepo.SelectWhere(sa => sa.Id == "RFC").ToList();

            Assert.IsNotNull(ssoApps);
            Assert.IsTrue(ssoApps.Any());
        }

        [TestMethod]
        public void GetFullSsoApplication()
        {
            var ssoApp = ssoApplicationRepo.Get(app => app.Id == "RFC", "Profiles");

            Assert.IsNotNull(ssoApp);
            Assert.IsNotNull(ssoApp.Id);
            Assert.IsTrue(ssoApp.Profiles.Any());
        }
    }
}
