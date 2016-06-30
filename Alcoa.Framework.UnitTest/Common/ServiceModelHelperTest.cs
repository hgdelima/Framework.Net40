using Alcoa.Framework.Common;
using Alcoa.Framework.Common.Presentation.Proxy;
using Alcoa.Framework.Contract.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alcoa.Framework.UnitTest.Common
{
    [TestClass]
    public class ServiceModelHelperTest
    {
        private string _contractName;

        [TestInitialize]
        public void ServiceModelHelperTestInitialize()
        {
            _contractName = typeof(ISsoService).Name;
        }

        [TestMethod]
        public void SearchClientEndpointByContractName()
        {
            var endpoint = ServiceModelHelper.SearchClientEndpointConfigs(_contractName);

            Assert.IsNotNull(endpoint);
        }
    }
}