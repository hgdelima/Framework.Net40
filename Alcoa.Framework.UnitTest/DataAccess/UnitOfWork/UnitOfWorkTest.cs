using Alcoa.Entity.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Data;

namespace Alcoa.Framework.UnitTest.DataAccess
{
    [TestClass]
    public class UnitOfWorkTest
    {
        private IUnitOfWork uow;

        [TestInitialize]
        public void Initialize()
        {
            uow = new UnitOfWork<CorporateContextFmw>();
        }

        [TestMethod]
        public void InitializeUnitOfWorkUsingContextType()
        {
            Assert.IsNotNull(uow);
        }

        [TestMethod]
        public void InitializeUnitOfWorkUsingProfileName()
        {
            uow = new UnitOfWork<CorporateContextFmw>("fmw");

            Assert.IsNotNull(uow);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void FailToInitializeUnitOfWorkUsingProfileName()
        {
            uow = new UnitOfWork<CorporateContextFmw>("wrongProfileCode");
        }

        [TestMethod]
        public void ExecuteNonQueryUsingUnitOfWorkConnection()
        {
            var result = uow.ExecuteNonQuery("select * from alcoa.tbg_colaborador where rownum <= 1");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ExecuteDatasetUsingUnitOfWorkConnection()
        {
            var ds = uow.ExecuteDataset("select * from alcoa.tbg_colaborador where rownum <= 10");

            Assert.IsNotNull(ds);
            Assert.IsNotNull(ds.Tables);
        }

        [TestMethod]
        public void ExecuteScalarUsingUnitOfWorkConnection()
        {
            var query = "SELECT T.CHAPA FROM ALCOA.TBG_COLABORADOR T WHERE T.LOGIN = :s_login";
            var parameters = new Hashtable();
            parameters.Add("s_login", "v-mussala");

            var sequence = uow.ExecuteScalar<string>(query, parameters);

            Assert.IsNotNull(sequence);
        }
    }
}
