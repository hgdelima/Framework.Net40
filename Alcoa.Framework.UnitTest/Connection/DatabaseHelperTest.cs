using Alcoa.Framework.Connection;
using Alcoa.Framework.Connection.Enumerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Data;

namespace Alcoa.Framework.UnitTest.Connection
{
    [TestClass]
    public class DatabaseHelperTest
    {
        private string _query;
        private string _connectionName;
        private string _connectionString;
        private string _loginParameter;

        [TestInitialize]
        public void DatabaseHelperTestInitialize()
        {
            _connectionName = "FMW";
            _connectionString = "DATA SOURCE=DBALCAD;PERSIST SECURITY INFO=False;USER ID=WEB_FRAMEWORKALCOA;Password=FRAMEWORKALCOA_Alcoa!#2014";
            _loginParameter = "v-mussala";
        }

        [TestMethod]
        public void TestExecuteDatasetWithConnectionProfileName()
        {
            _query = "SELECT * FROM ALCOA.TBG_COLABORADOR WHERE LOGIN = '" + _loginParameter + "'";
            var workersDs = new DatabaseHelper(_connectionName).ExecuteDataset(_query);

            Assert.IsNotNull(workersDs);
            Assert.IsTrue(workersDs.Tables.Count > 0);
        }

        [TestMethod]
        public void TestExecuteDatasetWithConnectionAndParameters()
        {
            _query = "SELECT T.* FROM ALCOA.TBG_COLABORADOR T WHERE T.LOGIN = :s_login";
            var parameters = new Hashtable();
            parameters.Add("s_login", _loginParameter);

            var ds = new DatabaseHelper(_connectionString, DatabaseTypes.Oracle).ExecuteDataset(_query, parameters);

            Assert.IsNotNull(ds);
            Assert.IsTrue(ds.Tables.Count > 0);
        }

        [TestMethod]
        public void TestExecuteNonQueryWithConnectionAndParameters()
        {
            _query = "SELECT T.* FROM ALCOA.TBG_COLABORADOR T WHERE T.LOGIN = :s_login";
            var parameters = new Hashtable();
            parameters.Add("s_login", _loginParameter);

            var affectedRows = new DatabaseHelper(_connectionString, DatabaseTypes.Oracle).ExecuteNonQuery(_query, parameters);

            Assert.IsNotNull(affectedRows);
            Assert.IsTrue(affectedRows < 0);
        }

        [TestMethod]
        public void TestExecuteScalarWithConnectionAndParameters()
        {
            _query = "SELECT T.* FROM ALCOA.TBG_COLABORADOR T WHERE T.LOGIN = :s_login";
            var parameters = new Hashtable();
            parameters.Add("s_login", _loginParameter);

            var obj = new DatabaseHelper(_connectionString, DatabaseTypes.Oracle).ExecuteScalar(_query, parameters);

            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void TestPackageWithInputAndOutputParameters()
        {
            _query = @"BI_EBS.PKG_CARREGAR_MIDPOINT.PR_MAIN";

            var parameters = new Hashtable();
            parameters.Add("P_LOGIN", "GUARIKH");
            parameters.Add("P_ID_JOB_EST", "02");
            parameters.Add("P_ID_JOB_CRP", "35/");
            parameters.Add("P_LBC", "02591");
            parameters.Add("P_ANO", "2016");
            parameters.Add("P_STATUS", null);
            parameters.Add("P_MSG", null);

            var obj = new DatabaseHelper("WBMS").ExecuteNonQuery(_query, parameters, CommandType.StoredProcedure);

            Assert.IsNotNull(obj);
            Assert.AreNotEqual(string.Empty, parameters["P_MSG"]);
            Assert.AreNotEqual(string.Empty, parameters["P_STATUS"]);
        }

        [TestMethod]
        public void TestTransctionCommiting()
        {
            _connectionName = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\v-mussala\Documents\Northwind.accdb;Persist Security Info=False;";

            using (var dh = new DatabaseHelper(_connectionName, DatabaseTypes.Access))
            {
                dh.BeginTransaction();

                _query = "insert into [Orders Status] ([Status ID], [Status Name]) values (4, 'TESTE 1')";
                var result = dh.ExecuteNonQuery(_query);

                var _query2 = "insert into [Orders Status] ([Status ID], [Status Name]) values (5, 'TESTE 2')";
                var result2 = dh.ExecuteNonQuery(_query2);

                dh.CommitTransaction();

                Assert.AreEqual(1, result);
                Assert.AreEqual(1, result2);
            }

            _query = "delete from [Orders Status] Where [Status ID] in (4, 5)";
            new DatabaseHelper(_connectionName, DatabaseTypes.Access).ExecuteNonQuery(_query);
        }

        [TestMethod]
        public void TestTransctionRollingBack()
        {
            _connectionName = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\v-mussala\Documents\Northwind.accdb;Persist Security Info=False;";

            using (var dh = new DatabaseHelper(_connectionName, DatabaseTypes.Access))
            {
                dh.BeginTransaction();

                _query = "insert into [Orders Status] ([Status ID], [Status Name]) values (4, 'TESTE 1')";
                var result = dh.ExecuteNonQuery(_query);

                var _query2 = "insert into [Orders Status] ([Status ID], [Status Name]) values (5, 'TESTE 2')";
                var result2 = dh.ExecuteNonQuery(_query2);

                dh.RollbackTransaction();

                var _query3 = "select * from [Orders Status] Where [Status ID] in (4, 5)";
                var result3 = dh.ExecuteDataset(_query3);

                Assert.AreEqual(1, result);
                Assert.AreEqual(1, result2);
                Assert.IsNotNull(result3);
                Assert.IsTrue(result3.Tables.Count > 0);
                Assert.IsTrue(result3.Tables[0].Rows.Count <= 0);
            }
        }
    }
}
