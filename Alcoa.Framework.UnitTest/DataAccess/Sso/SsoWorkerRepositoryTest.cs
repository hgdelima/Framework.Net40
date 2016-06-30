using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alcoa.Framework.UnitTest.DataAccess.Sso
{
    [TestClass]
    public class SsoWorkerRepositoryTest
    {
        private IUnitOfWork uow;
        private IRepository<Worker> workerRepo;

        [TestInitialize]
        public void Initialize()
        {
            uow = new UnitOfWork<SsoContextSso>();
            workerRepo = uow.GetRepository<Worker>();
        }

        #region Worker

        [TestMethod]
        public void GetWorker()
        {
            var worker = workerRepo.Get(w => w.Login == "ferrewc");

            Assert.IsNotNull(worker);
        }

        #endregion
    }
}
