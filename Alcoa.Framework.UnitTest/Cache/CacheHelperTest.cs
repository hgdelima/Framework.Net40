using Alcoa.Entity.Entity;
using Alcoa.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace Alcoa.Framework.UnitTest.Common
{
    [TestClass]
    public class CacheHelperTest
    {
        private BaseIdentification cachedObject = new BaseIdentification("78", "Luiz Mussa");
        private string cachedString= "Luiz Mussa";

        [TestInitialize]
        public void CacheHelperTestInitialize()
        {
            cachedObject.AddToCache("cachedKeyComplex", DateTime.Now.AddSeconds(10));
            cachedString.AddToCache("cachedKeyString", DateTime.Now.AddSeconds(10));
        }

        [TestMethod]
        public void GetSimpleObjectCache()
        {
            var stringObj = CacheHelper.GetFromCache<string>("cachedKeyString");

            Assert.IsNotNull(stringObj);
            Assert.IsTrue(stringObj == cachedString);
        }

        [TestMethod]
        public void GetComplexObjectCache()
        {
            var obj = CacheHelper.GetFromCache<BaseIdentification>("cachedKeyComplex");

            Assert.IsNotNull(obj);
            Assert.IsTrue(obj is BaseIdentification);
        }

        [TestMethod]
        public void CheckCacheExpiration()
        {
            Thread.Sleep(10000);
            var obj = CacheHelper.GetFromCache<BaseIdentification>("cachedKeyComplex");

            Assert.IsNull(obj);
        }

        [TestMethod]
        public void GetEmptyObjectCache()
        {
            var stringObj = CacheHelper.GetFromCache<string>("cachedKeyEmpty");
            var complexObj = CacheHelper.GetFromCache<BaseIdentification>("cachedKeyEmpty");

            Assert.IsNull(stringObj);
            Assert.IsTrue(complexObj is BaseIdentification);
        }
    }
}
