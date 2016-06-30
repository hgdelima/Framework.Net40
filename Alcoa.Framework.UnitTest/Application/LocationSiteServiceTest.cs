using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Application.Service;
using Alcoa.Framework.Common.Entity;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alcoa.Framework.UnitTest.Application
{
    [TestClass]
    public class LocationSiteServiceTest
    {
        private LocationSiteService _locationService;

        [TestInitialize]
        public void LocationSiteServiceTestInitialize()
        {
            _locationService = new LocationSiteService();
        }

        [TestMethod]
        public void InstantianteCorporateService()
        {
            //var teste = container.GetInstance<IRepository<Collaborator>>();
            //var service = new AdminService(teste, teste);

            //service.GetData(123);
        }
    }
}
