using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Alcoa.Framework.UnitTest.DataAccess
{
    [TestClass]
    public class WorkerRepositoryTest
    {
        private IUnitOfWork uow;
        private IRepository<Worker> workerRepo;

        [TestInitialize]
        public void Initialize()
        {
            uow = new UnitOfWork<CorporateContextFmw>();
            workerRepo = uow.GetRepository<Worker>();
        }

        [TestMethod]
        public void GetWorker()
        {
            var worker = workerRepo.SelectWhere(w => w.Email == "luiz.mussa@alcoa.com.br");

            Assert.IsNotNull(worker);
        }

        [TestMethod]
        public void GetFiftyWorkers()
        {
            var workers = new List<Worker>();

            for (var i = 0; i < 50; i++)
                workers.Add(workerRepo.Get(w => w.Login == "v-mussala"));

            Assert.IsTrue(workers.Count == 50);
        }

        [TestMethod]
        public void GetFullWorker()
        {
            var worker = workerRepo.Get(w => w.Id == "1005547",
                "Manager",
                "Manager.Manager",
                "Employees",
                "Employees.Employee",
                "BudgetCode",
                "BudgetCode.Area",
                "BudgetCode.Site",
                "BudgetCode.Dept",
                "BudgetCode.Lbc");

            Assert.IsNotNull(worker);
            Assert.IsNotNull(worker.BudgetCode);
            Assert.IsNotNull(worker.Manager);
            Assert.IsNotNull(worker.Employees);
        }
    }
}
