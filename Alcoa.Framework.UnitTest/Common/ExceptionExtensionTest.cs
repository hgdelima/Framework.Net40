using Alcoa.Framework.Common;
using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel;

namespace Alcoa.Framework.UnitTest.Common
{
    [TestClass]
    public class ExceptionExtensionTest
    {
        [TestInitialize]
        public void ExceptionExtensionTestInitialize()
        {
        }

        [TestMethod]
        public void GetServiceExceptionValidationsMessage()
        {
            var exMessage = string.Empty;

            try
            {
                var listEx = new List<ValidationResult>();
                listEx.Add(new ValidationResult("Error 1"));
                listEx.Add(new ValidationResult("Error 2"));
                listEx.Add(new ValidationResult("Error 3"));

                throw new ServiceException(listEx);
            }
            catch (FaultException ex)
            {
                exMessage = ex.GetAllExceptionMessages(); 
            }

            Assert.IsNotNull(exMessage);
            Assert.IsTrue(exMessage.Contains("Error"));
        }

        [TestMethod]
        public void GetServiceExceptionCommonMessage()
        {
            var exMessage = string.Empty;

            try
            {
                throw new ServiceException(CommonExceptionType.ParameterException, new Exception("Name"));
            }
            catch (FaultException ex)
            {
                exMessage = ex.GetAllExceptionMessages();
            }

            Assert.IsNotNull(exMessage);
            Assert.IsTrue(exMessage.Contains("Invalid"));
        }
    }
}