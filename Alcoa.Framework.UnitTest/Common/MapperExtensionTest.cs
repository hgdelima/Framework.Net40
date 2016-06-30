using System;
using System.Collections.Generic;
using Alcoa.Entity.Entity;
using Alcoa.Framework.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alcoa.Framework.UnitTest.Common
{
    [TestClass]
    public class MapperExtensionTest
    {
        #region Source object

        public class ModelObject
        {
            public DateTime? BaseDate { get; set; }
            public string Name { get; set; }
            public ModelSubObject Sub { get; set; }

            public List<ModelObject> InnerList { get; set; }
        }

        public class ModelSubObject
        {
            public string ProperName { get; set; }
            public List<ModelObject> ModelList { get; set; }
        }

        #endregion

        #region Target object

        public class ModelDto
        {
            public DateTime? BaseDate { get; set; }
            public string Name { get; set; }
            public DateTime? SubNullableDate { get; set; }
            public DateTime? Sub2NullableDate { get; set; }
            public string SubProperName { get; set; }
            public ModelSubDTO Sub { get; set; }

            public List<ModelDto> InnerList { get; set; }
        }

        public class ModelSubDTO
        {
            public string ProperName { get; set; }
            public List<ModelDto> ModelList { get; set; }
        }

        #endregion

        public ModelObject CreateNewComplexObject()
        {
            return new ModelObject
            {
                BaseDate = DateTime.Now,
                Sub = new ModelSubObject
                {
                    ProperName = "Some name",
                    ModelList = new List<ModelObject> 
                    {
                        new ModelObject { Name = "B" },
                        new ModelObject { Name = "B" },
                    },
                },
                InnerList = new List<ModelObject> 
                {
                    new ModelObject { Name = "A" },
                    new ModelObject { Name = "A" },
                    new ModelObject { Name = "A" },
                }
            };
        }

        [TestMethod]
        public void MapSimpleObject()
        {
            var user = new BaseUser { Login = "Test", Email = "test@test.com" };
            var userMapped = user.Map<BaseUser, BaseUser>();

            Assert.IsNotNull(userMapped);
            Assert.AreEqual("Test", userMapped.Login);
        }

        [TestMethod]
        public void MapComplexObject()
        {
            var source = CreateNewComplexObject();
            var modelDto = source.Map<ModelObject, ModelDto>();

            Assert.IsNotNull(modelDto);
        }

        [TestMethod]
        public void MapListObject()
        {
            var source = CreateNewComplexObject();
            var modelDtoList = source.InnerList.Map<List<ModelObject>, List<ModelDto>>();

            Assert.IsNotNull(modelDtoList);
        }
    }
}