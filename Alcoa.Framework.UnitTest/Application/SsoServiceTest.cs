using Alcoa.Framework.Application.Service;
using Alcoa.Framework.Common;
using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Contract.DTOs.Sso;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alcoa.Framework.UnitTest.Application
{
    [TestClass]
    public class SsoServiceTest
    {
        private SsoService _ssoService;

        [TestInitialize]
        public void ApplicationTestInitialize()
        {
            _ssoService = new SsoService();
        }

        [TestMethod]
        public void GetUserOperation()
        {
            var user = _ssoService.GetUser(new UserFilterDTO
            {
                Login = "sousajs",
                LoadTranslations = true
            });

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void ValidateAndGetUserAuthorizationsOperation()
        {
            var auth = _ssoService.ValidateAndGetUserAuthorizations(new SsoAuthenticationDTO
            {
                EncriptedLogin = CryptographHelper.RijndaelEncrypt("v-mussala", CommonConsts.CommonPassword),
                EncriptedPassword = CryptographHelper.RijndaelEncrypt("Songoku&*78", CommonConsts.CommonPassword),
                EncriptedAppCode = CryptographHelper.RijndaelEncrypt("SCB", CommonConsts.CommonPassword),
                LanguageCultureName = "EN-US"
            });

            Assert.IsNotNull(auth);
        }
    }
}
