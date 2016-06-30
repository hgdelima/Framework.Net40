using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace Alcoa.Framework.UnitTest.DataAccess
{
    [TestClass]
    public class ActiveDirectoryRepositoryTest
    {
        private IUnitOfWork uow;
        private IActiveDirectoryRepository adRepository;

        [TestInitialize]
        public void ActiveDirectoryRepositoryInitialize()
        {
            uow = new UnitOfWork<CorporateContextFmw>();
            adRepository = uow.GetRepositoryActiveDirectory();
        }

        [TestMethod]
        public void GetUser()
        {
            var adUser = adRepository.GetUser("sousajs", false);

            Assert.IsNotNull(adUser);
            Assert.AreEqual("sousajs", adUser.Login);
        }

        [TestMethod]
        public void GetUserFull()
        {
            var adUser = adRepository.GetUser("sousajs", true, true);

            Assert.IsNotNull(adUser);
            Assert.AreEqual("Joao", adUser.UserExtraInfo.FirstName);
        }

        [TestMethod]
        public void GetFiftyUsers()
        {
            var adUser = default(BaseUser);
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 50; i++)
                adUser = adRepository.GetUser("sousajs", false, false);

            sw.Stop();

            Assert.IsNotNull(sw.Elapsed);
            Assert.IsNotNull(adUser);
            Assert.AreEqual("sousajs", adUser.Login);
        }

        [TestMethod]
        public void SearchUsers()
        {
            var adUserList = adRepository.SearchUsers("Joao");

            Assert.IsNotNull(adUserList);
            Assert.IsTrue(adUserList.Any());
        }

        [TestMethod]
        public void SearchUsersAtSpecificDomain()
        {
            var adUserList = adRepository.SearchUsers("Luiz", "SOUTH_AMERICA");

            Assert.IsNotNull(adUserList);
            Assert.IsTrue(adUserList.Any());
        }

        [TestMethod]
        public void GetGroup()
        {
            var adGroup = adRepository.GetGroup("SOA DSI Technical Team DL", true);

            Assert.IsNotNull(adGroup);
            Assert.AreEqual("SOA DSI Technical Team DL", adGroup.NameOrDescription);
        }

        [TestMethod]
        public void SearchGroups()
        {
            var adGroupList = adRepository.SearchGroups("DSI");

            Assert.IsNotNull(adGroupList);
            Assert.IsTrue(adGroupList.Any());
        }

        [TestMethod]
        public void SearchGroupsAtSpecificDomain()
        {
            var adGroupList = adRepository.SearchGroups("DSI", "SOUTH_AMERICA");

            Assert.IsNotNull(adGroupList);
            Assert.IsTrue(adGroupList.Any());
        }

        [TestMethod]
        public void GetDomainList()
        {
            var domains = adRepository.GetDomainList();

            Assert.IsNotNull(domains);
            Assert.IsTrue(domains.Any());
        }

        [TestMethod]
        public void ValidateUserCredential()
        {
            var isValidUser = adRepository.ValidateUserCredential("sousajs", "*SenhaTeste*");

            Assert.Inconclusive(isValidUser.ToString());
        }

        [TestMethod]
        public void UnlockUserAccount()
        {
            adRepository.UnlockUserAccount("sousajs");

            Assert.Inconclusive();
        }
    }
}