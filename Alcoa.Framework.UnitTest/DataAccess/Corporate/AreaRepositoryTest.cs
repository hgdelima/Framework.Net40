using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Alcoa.Framework.UnitTest.DataAccess
{
    [TestClass]
    public class AreaRepositoryTest
    {
        private IUnitOfWork uow;
        private IRepository<Area> areaRepo;

        [TestInitialize]
        public void Initialize()
        {
            uow = new UnitOfWork<CorporateContextFmw>();
            areaRepo = uow.GetRepository<Area>();
        }

        #region Area

        [TestMethod]
        public void GetArea()
        {
            var area = areaRepo.Get(a => a.AreaId == "IT" && a.SiteId == "PFA");

            Assert.IsNotNull(area);
        }

        [TestMethod]
        public void GetAreaFull()
        {
            var area = areaRepo.Get(a => 
                a.AreaId == "IT" && 
                a.SiteId == "GBS", 
                "Manager", "Site", "AreaParent", "SubAreas", "BudgetCodes");

            Assert.IsNotNull(area);
        }

        [TestMethod]
        public void GetAreaDataList()
        {
            var areaList = areaRepo.SelectWhere(s => s.IsActive == "S");

            Assert.IsNotNull(areaList);
            Assert.IsTrue(areaList.Any());
        }

        #endregion
    }
}
