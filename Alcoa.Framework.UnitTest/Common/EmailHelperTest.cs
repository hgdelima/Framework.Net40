using Alcoa.Entity.Entity;
using Alcoa.Framework.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Alcoa.Framework.UnitTest.Common
{
    [TestClass]
    public class EmailHelperTest
    {
        private BaseEmail email;

        [TestInitialize]
        public void EmailHelperTestInitialize()
        {
            email = new BaseEmail
            {
                Subject = "Alcoa Unit Test",
                From = "alcoa@framework.com.br",
                FromName = "Alcoa Unit Test",
                To = "luiz.mussa@alcoa.com.br;",
                IsHtml = true,
                Body = "Unit Test<br />Unit Test<br />Unit Test<br />Unit Test<br />Unit Test<br />Unit Test<br />"
            };
        }

        [TestMethod]
        public void SendEmailToOne()
        {
            var emailResponse = EmailHelper.Send(email);

            Assert.IsNotNull(emailResponse);
            Assert.IsTrue(string.IsNullOrEmpty(emailResponse));
        }

        [TestMethod]
        public void SendEmailToMany()
        {
            email.Subject = "Alcoa Unit Test To Many Emails";
            email.To += "marcio.freitas@alcoa.com.br";
            var emailResponse = EmailHelper.Send(email);

            Assert.IsNotNull(emailResponse);
            Assert.IsTrue(string.IsNullOrEmpty(emailResponse));
        }
    }
}
