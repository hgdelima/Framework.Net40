using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Alcoa.Framework.UnitTest.DataAccess
{
    [TestClass]
    public class SiteRepositoryTest
    {
        private IUnitOfWork uow;
        private IRepository<Site> siteRepo;

        [TestInitialize]
        public void Initialize()
        {
            uow = new UnitOfWork<CorporateContextFmw>();
            siteRepo = uow.GetRepository<Site>();
        }

        [TestMethod]
        public void GetSiteData()
        {
            var site = siteRepo.Get(s => s.Id == "PFA", "Lbcs", "Areas");

            Assert.IsNotNull(site);
            Assert.IsNotNull(site.Lbcs);
            Assert.IsTrue(site.Lbcs.Count > 1);
        }

        [TestMethod]
        public void GetSiteDataList()
        {
            var siteList = siteRepo.SelectWhere(s => s.IsActive == "S", "Lbcs");

            Assert.IsNotNull(siteList);
            Assert.IsTrue(siteList.Any());
        }

        [TestMethod]
        public void GetAllSitesWithoutFilter()
        {
            var siteList = siteRepo.SelectWhere().ToList();

            Assert.IsNotNull(siteList);
            Assert.IsTrue(siteList.Any());
        }
    }
}
