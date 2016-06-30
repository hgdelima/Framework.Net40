using Alcoa.Framework.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.IO;

namespace Alcoa.Framework.UnitTest.Common
{
    [TestClass]
    public class FileHelperTest
    {
        [TestInitialize]
        public void FileHelperTestInitialize()
        {
        }

        [TestMethod]
        public void GetFilesAtPath()
        {
            var files = FileHelper.GetFiles(@"\\soautg-hub01\temporary\LuizMussa");

            Assert.IsNotNull(files);
            Assert.IsTrue(files.Count > 0);
        }

        [TestMethod]
        public void GetFilesUsingExtensionAndLimitNumber()
        {
            var files = FileHelper.GetFiles(@"\\soautg-hub01\temporary\LuizMussa", "*.job", SearchOption.TopDirectoryOnly, 10);

            Assert.IsNotNull(files);
            Assert.IsTrue(files.Count > 0);
        }

        [TestMethod]
        public void GetFilesAtPathUsingCredential()
        {
            var files = FileHelper.GetFilesUsingCredential(@"\\soautg-hub01\temporary\LuizMussa", "v-mussala", "**ChangeHere***", "SOUTH_AMERICA");

            Assert.IsNotNull(files);
            Assert.IsTrue(files.Count > 0);
        }

        [TestMethod]
        public void GetAndReadFilesUsingExtensionAndLimitNumber()
        {
            var files = FileHelper.GetFiles(@"\\soautg-hub01\temporary\LuizMussa", "*.job", SearchOption.TopDirectoryOnly, 10);
            var fileLength = default(int);

            foreach (var file in files)
            {
                using (var fileText = file.OpenText())
                {
                    fileLength = fileText.ReadToEnd().Length;
                }
            }

            Assert.IsNotNull(files);
            Assert.IsTrue(files.Count > 0);
            Assert.IsTrue(fileLength > 0);
        }

        [TestMethod]
        public void ReadFilesUsingExtensionAndLimitNumber()
        {
            var files = FileHelper.OpenFiles(@"\\soautg-hub01\temporary\LuizMussa", "*.job", SearchOption.TopDirectoryOnly, 10);

            Assert.IsNotNull(files);
            Assert.IsTrue(files.Count > 0);
        }
    }
}
