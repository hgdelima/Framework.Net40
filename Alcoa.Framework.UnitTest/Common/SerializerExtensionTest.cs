using System;
using System.Collections.Generic;
using Alcoa.Entity.Entity;
using Alcoa.Framework.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alcoa.Framework.Domain.Entity;
using Alcoa.Framework.Contract.DTOs;

namespace Alcoa.Framework.UnitTest.Common
{
    [Serializable]
    public class WorkerXML : WorkerDTO
    {
    }

    [TestClass]
    public class SerializerExtensionTest
    {
        private Worker jsonObject;
        private WorkerXML xmlObject;
        private WorkerDTO datacontractObject;

        [TestInitialize]
        public void SerializerExtensionTestInitialize()
        {
            jsonObject = new Worker
            {
                Id = "1",
                NameOrDescription = "Luiz Arthur P. Mussa",
                AssociationType = "Teste",
                BirthDate = DateTime.Now,
                BranchLine = "0800787878",
                BusinessTitle = "Application Architect",
                DeptId = "1",
                LbcId = "1",
                Login = "v-mussala",
                Applications = new List<SsoApplication>
                {
                    new SsoApplication {
                        Id = "PORTARIA",
                        HomeUrl = "www.dev-intra.soa.alcoa.com/portaria",
                        Mnemonic = "PORTARIA",
                        Profiles = new List<SsoProfile>
                        {
                            new SsoProfile {
                                Id = "1",
                                ApplicationId = "PORTARIA",
                                IsPublic = "N",
                                NameOrDescription = "Teste",
                            }
                        }
                    }
                }
            };

            xmlObject = jsonObject.Map<Worker, WorkerXML>();
            datacontractObject = jsonObject.Map<Worker, WorkerDTO>();  
        }

        [TestMethod]
        public void SerializeAndDeserializeToJson()
        {
            var serializedObj = jsonObject.Serialize();
            var deserializedObj = serializedObj.Deserialize<Worker>();

            Assert.IsNotNull(serializedObj);
            Assert.IsNotNull(deserializedObj);
        }

        [TestMethod]
        public void SerializeAndDeserializeToDataContract()
        {
            var serializedObj = datacontractObject.Serialize();
            var deserializedObj = serializedObj.Deserialize<WorkerDTO>();

            Assert.IsNotNull(serializedObj);
            Assert.IsNotNull(deserializedObj);
        }

        [TestMethod]
        public void SerializeAndDeserializeXmlObject()
        {
            var serializedObj = xmlObject.Serialize();
            var deserializedObj = serializedObj.Deserialize<WorkerXML>();

            Assert.IsNotNull(serializedObj);
            Assert.IsNotNull(deserializedObj);
        }
    }
}