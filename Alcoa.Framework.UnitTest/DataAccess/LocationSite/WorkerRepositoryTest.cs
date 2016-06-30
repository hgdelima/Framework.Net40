using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alcoa.Framework.UnitTest.DataAccess.LocationSite
{
    [TestClass]
    public class WorkerRepositoryTest
    {
        private IUnitOfWork uow;
        private IRepository<Worker> workerRepo;

        [TestInitialize]
        public void Initialize()
        {
            uow = new UnitOfWork<LocationSiteContextFmw>("FMWUTGBD");
            workerRepo = uow.GetRepository<Worker>();
        }

        #region Worker

        [TestMethod]
        public void GetWorker()
        {
            var worker = workerRepo.Get(w => w.Login.ToLower() == "v-mussala");

            Assert.IsNotNull(worker);
            Assert.IsTrue(!string.IsNullOrEmpty(worker.Id));
        }

        #endregion
    }
}
