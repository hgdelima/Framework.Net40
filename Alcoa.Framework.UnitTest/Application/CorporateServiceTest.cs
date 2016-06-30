using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Application.Service;
using Alcoa.Framework.Common.Entity;
using Alcoa.Framework.Contract.DTOs.Corporate;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alcoa.Framework.UnitTest.Application
{
    [TestClass]
    public class CorporateServiceTest
    {
        private CorporateService _corporateService;

        [TestInitialize]
        public void LocationSiteServiceTestInitialize()
        {
            _corporateService = new CorporateService();
        }

        [TestMethod]
        public void TwentyTimesServicesCalls()
        {
            for(int i = 1; i <= 20; i++)
            {
                _corporateService = new CorporateService();

                var user = _corporateService.GetWorker(new WorkerFilterDTO { Login = "v-mussala" });

                Assert.IsNotNull(user);
            }
        }
    }
}
