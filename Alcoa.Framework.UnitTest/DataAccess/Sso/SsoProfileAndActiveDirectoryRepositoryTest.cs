using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alcoa.Framework.UnitTest.DataAccess
{
    [TestClass]
    public class SsoProfileAndActiveDirectoryRepositoryTest
    {
        private IUnitOfWork uow;
        private IRepository<SsoProfileAndActiveDirectory> profilesAndAdsRepo;

        [TestInitialize]
        public void Initialize()
        {
            uow = new UnitOfWork<SsoContextSso>();
            profilesAndAdsRepo = uow.GetRepository<SsoProfileAndActiveDirectory>();
        }

        [TestMethod]
        public void GetSsoProfileAndActiveDirectory()
        {
            var ssoProfileAndAd = profilesAndAdsRepo.Get(w => w.ApplicationId == "RFC");

            Assert.IsNotNull(ssoProfileAndAd);
            Assert.IsNotNull(ssoProfileAndAd.ApplicationId);
            Assert.IsNotNull(ssoProfileAndAd.ProfileId);
            Assert.IsNotNull(ssoProfileAndAd.AdGroupId);
        }

        [TestMethod]
        public void GetFullSsoProfileAndActiveDirectory()
        {
            var ssoProfileAndAd = profilesAndAdsRepo.Get(w =>
                w.ApplicationId == "RFC" &&
                w.ProfileId == "1",
                "AdGroup");

            Assert.IsNotNull(ssoProfileAndAd);
            Assert.IsNotNull(ssoProfileAndAd.AdGroup);
        }
    }
}
