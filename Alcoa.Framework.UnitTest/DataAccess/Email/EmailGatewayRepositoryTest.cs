using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Alcoa.Framework.UnitTest.DataAccess
{
    [TestClass]
    public class EmailGatewayRepositoryTest
    {
        private IUnitOfWork uow;
        private IRepository<EmailGateway> emailRepo;

        [TestInitialize]
        public void Initialize()
        {
            uow = new UnitOfWork<EmailContextSsm>();
            emailRepo = uow.GetRepository<EmailGateway>();
        }

        [TestMethod]
        public void GetGatewayEmail()
        {
            var email = emailRepo.Get(sa => sa.IsProcessed == "N");

            Assert.IsNotNull(email);
            Assert.IsNotNull(email.CurrentCode);
            Assert.IsNotNull(email.JobSequence);
        }

        [TestMethod]
        public void GetGatewayEmails()
        {
            var emails = emailRepo.SelectWhere(sa => 
                sa.IsProcessed == "S" && 
                sa.IsEvent == "N")
                .Take(100)
                .ToList();

            Assert.IsNotNull(emails);
            Assert.IsTrue(emails.Count > 0);
        }

        [TestMethod]
        public void UpdateGatewayEmail()
        {
            var email = emailRepo.Get(sa => sa.IsProcessed == "N");
            var wasUpdated = emailRepo.Update(email);

            uow.Commit();

            Assert.IsNotNull(wasUpdated);
            Assert.IsTrue(wasUpdated);
        }
    }
}
