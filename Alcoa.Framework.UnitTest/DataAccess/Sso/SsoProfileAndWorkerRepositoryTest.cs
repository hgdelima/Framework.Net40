using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Alcoa.Framework.UnitTest.DataAccess
{
    [TestClass]
    public class SsoProfileAndWorkerRepositoryTest
    {
        private IUnitOfWork uow;
        private IRepository<SsoProfileAndWorker> profileAndWorkersRepo;

        [TestInitialize]
        public void Initialize()
        {
            uow = new UnitOfWork<SsoContextSso>();
            profileAndWorkersRepo = uow.GetRepository<SsoProfileAndWorker>();
        }

        [TestMethod]
        public void GetSsoProfileAndWorker()
        {
            var ssoWorker = profileAndWorkersRepo.Get(w => w.Login == "v-mussala");

            Assert.IsNotNull(ssoWorker);
            Assert.IsNotNull(ssoWorker.ApplicationId);
            Assert.IsNotNull(ssoWorker.ProfileId);
            Assert.IsNotNull(ssoWorker.WorkerOrEmployeeId);
        }

        [TestMethod]
        public void GetSsoProfileAndWorkerList()
        {
            var ssoWorkers = profileAndWorkersRepo.SelectWhere(w =>
                w.Login == null ||
                w.Login == "v-mussala");

            Assert.IsNotNull(ssoWorkers);
            Assert.IsTrue(ssoWorkers.Any());
        }

        [TestMethod]
        public void GetFullSsoProfileAndWorkersList()
        {
            var ssoWorkers = profileAndWorkersRepo.SelectWhere(w => w.Login == "v-mussala",
                "Profile")
                .ToList();

            Assert.IsNotNull(ssoWorkers);
            Assert.IsTrue(ssoWorkers.Any());
        }
    }
}
