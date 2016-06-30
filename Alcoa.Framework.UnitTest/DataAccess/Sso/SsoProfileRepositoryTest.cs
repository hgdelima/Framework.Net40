using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Alcoa.Framework.UnitTest.DataAccess
{
    [TestClass]
    public class SsoProfileRepositoryTest
    {
        private IUnitOfWork uow;
        private IRepository<SsoProfile> ssoProfileRepo;

        [TestInitialize]
        public void Initialize()
        {
            uow = new UnitOfWork<SsoContextSso>();
            ssoProfileRepo = uow.GetRepository<SsoProfile>();
        }

        [TestMethod]
        public void GetSsoProfile()
        {
            var ssoProfile = ssoProfileRepo.Get(w => w.ApplicationId == "RFC");

            Assert.IsNotNull(ssoProfile);
            Assert.IsNotNull(ssoProfile.ApplicationId);
            Assert.IsNotNull(ssoProfile.Id);
        }

        [TestMethod]
        public void GetSsoProfileList()
        {
            var ssoProfiles = ssoProfileRepo.SelectWhere(w => w.ApplicationId == "RFC");

            Assert.IsNotNull(ssoProfiles);
            Assert.IsTrue(ssoProfiles.Any());
        }

        [TestMethod]
        public void GetFullSsoProfile()
        {
            var ssoProfile = ssoProfileRepo.Get(w => w.ApplicationId == "RFC",
                "ProfilesAndActiveDirectories",
                "ProfilesAndActiveDirectories.AdGroup",
                "ProfilesAndServices", 
                "ProfilesAndServices.Service",
                "ProfilesAndServices.Service.SsoGroup");

            Assert.IsNotNull(ssoProfile);
            Assert.IsNotNull(ssoProfile.ApplicationId);
            Assert.IsNotNull(ssoProfile.Id);
            Assert.IsTrue(ssoProfile.ProfilesAndActiveDirectories.Any());
            Assert.IsTrue(ssoProfile.ProfilesAndServices.Any());
            Assert.IsNotNull(ssoProfile.ProfilesAndServices.First(s => s.Service != null).Service);
        }
    }
}
