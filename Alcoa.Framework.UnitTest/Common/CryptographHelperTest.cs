using Alcoa.Framework.Common;
using Alcoa.Framework.Common.Enumerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Alcoa.Framework.UnitTest.Common
{
    [TestClass]
    [DeploymentItem(@"Alcoa.Framework.UnitTest\Certificates\UnitTestCertificate.cer", "Certificates")]
    [DeploymentItem(@"Alcoa.Framework.UnitTest\Certificates\UnitTestCertificate.pfx", "Certificates")]
    public class CryptographHelperTest
    {
        private string _textToEncrypt;
        private string _password;
        private string _testCertificate;

        [TestInitialize]
        public void CryptographTestInitialize()
        {
            _textToEncrypt = "Text to ensure about cryptograph helper functions";
            _password = CommonConsts.CommonPassword;
            _testCertificate = Path.GetFullPath(@"Certificates\UnitTestCertificate.pfx");
        }

        [TestMethod]
        public void EncryptDataUsingRijndael()
        {
            var encriptedText = CryptographHelper.RijndaelEncrypt(_textToEncrypt, _password);
            var decriptedText = CryptographHelper.RijndaelDecrypt(encriptedText, _password);

            Assert.IsNotNull(encriptedText);
            Assert.IsNotNull(decriptedText);
            Assert.AreEqual(_textToEncrypt, decriptedText);
        }

        [TestMethod]
        public void EncryptDataUsingTripleDES()
        {
            var encriptedText = CryptographHelper.TripleDESEncrypt(_textToEncrypt, _password);
            var decriptedText = CryptographHelper.TripleDESDecrypt(encriptedText, _password);

            Assert.IsNotNull(encriptedText);
            Assert.IsNotNull(decriptedText);
            Assert.AreEqual(_textToEncrypt, decriptedText);
        }

        [TestMethod]
        public void EncryptAndDecryptDataUsingRSAKeys()
        {
            var publicKey = "123";
            var privateKey = "123";

            var encriptedText = CryptographHelper.RsaEncrypt(_textToEncrypt, publicKey);
            var decriptedText = CryptographHelper.RsaDecrypt(encriptedText, privateKey);

            Assert.IsNotNull(encriptedText);
            Assert.IsNotNull(decriptedText);
            Assert.AreEqual(_textToEncrypt, decriptedText);
        }

        [TestMethod]
        public void EncryptAndDecryptDataUsingRSACertificate()
        {
            var certificate = new X509Certificate2(X509Certificate2.CreateFromSignedFile(_testCertificate));

            var encriptedText = CryptographHelper.RsaEncrypt(_textToEncrypt, certificate);
            var decriptedText = CryptographHelper.RsaDecrypt(encriptedText, certificate);

            Assert.IsNotNull(encriptedText);
            Assert.IsNotNull(decriptedText);
            Assert.AreEqual(_textToEncrypt, decriptedText);
        }
    }
}
