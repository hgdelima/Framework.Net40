using Alcoa.Framework.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Alcoa.Framework.UnitTest.Common
{
    [TestClass]
    public class LogHelperTest
    {
        private string message;

        [TestInitialize]
        public void LogHelperTestInitialize()
        {
            message = "Test message to be logged";
        }

        [TestMethod]
        public void LogException()
        {
            var log = LogHelper.Exception(new Exception(message));

            Assert.IsNotNull(log);
        }

        [TestMethod]
        public void LogToEventViewer()
        {
            LogHelper.ToEventViewer(message);
        }

        [TestMethod]
        public void LogToFile()
        {
            LogHelper.ToFile(message);
        }

        [TestMethod]
        public void LogToMail()
        {
            LogHelper.ToEmail(message);
        }

        [TestMethod]
        public void LogToDatabase()
        {
            //LogHelper.ToDatabase(message);

            Assert.Inconclusive("Must be setuped at NLog.config");
        }
    }
}
