using Alcoa.Framework.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.IO;

namespace Alcoa.Framework.UnitTest.Common
{
    [TestClass]
    public class CastExtensionTest
    {
        public enum EnumTest
        {
            TestOne = 'O',
            TestTwo = 'T'
        }

        [TestInitialize]
        public void CastExtensionTestInitialize()
        {
        }
    }
}
