using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alcoa.Framework.UnitTest.DataAccess
{
    [TestClass]
    public class BudgetCodeRepositoryTest
    {
        private IUnitOfWork uow;
        private IRepository<BudgetCode> budgetRepo;

        [TestInitialize]
        public void Initialize()
        {
            uow = new UnitOfWork<CorporateContextFmw>();
            budgetRepo = uow.GetRepository<BudgetCode>();
        }

        #region Budget Code

        [TestMethod]
        public void GetBudgetCode()
        {
            var budget = budgetRepo.Get(b => 
                b.SiteId == "PFA" && 
                b.AreaId == "IT" &&
                b.LbcId == "03681" &&
                b.DeptId == "95900");

            Assert.IsNotNull(budget);
        }

        [TestMethod]
        public void GetBudgetCodeFull()
        {
            var budget = budgetRepo.Get(b =>
                b.SiteId == "PFA" && 
                b.AreaId == "REF" &&
                b.LbcId == "03682" &&
                b.DeptId == "03430",
                "Area", "Lbc", "Dept", "Manager");

            Assert.IsNotNull(budget);
            Assert.IsNotNull(budget.Area);
            Assert.IsNotNull(budget.Lbc);
            Assert.IsNotNull(budget.Dept);
            Assert.IsNotNull(budget.Manager);
        }

        #endregion
    }
}
