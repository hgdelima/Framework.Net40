using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps manipulate object Serializations
    /// </summary>
    public static class SerializerExtension
    {
        /// <summary>
        /// Serializes an object as string
        /// </summary>
        public static string Serialize<T>(this T obj) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var customAttribs = typeof(T).GetCustomAttributes(false);

            if (customAttribs.Length > 0)
            {
                if (customAttribs.Any(ca => ca.GetType() == typeof(SerializableAttribute)))
                    return SerializeXml(obj);
                else
                    return SerializeContract(obj);
            }                

            return SerializeJson(obj);
        }

        /// <summary>
        /// Deserializes an object to an specific typed object
        /// </summary>
        public static T Deserialize<T>(this string obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            //Checks if it was used XmlSerializer or DataContractSerializer
            if (obj.StartsWith("<"))
            {
                if (obj.StartsWith("<?xml version="))
                    return DeserializeXml<T>(obj);
                else
                    return DeserializeContract<T>(obj);
            }

            return DeserializeJson<T>(obj.ToString());
        }

        private static string SerializeXml<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var ms = new MemoryStream())
            {
                serializer.Serialize(ms, obj);
                return Encoding.Default.GetString(ms.ToArray());
            }
        }

        private static T DeserializeXml<T>(string contract)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var ms = new MemoryStream(Encoding.Default.GetBytes(contract)))
            {
                return (T)serializer.Deserialize(ms);
            }
        }

        private static string SerializeContract<T>(T obj)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                return Encoding.Default.GetString(ms.ToArray());
            }
        }

        private static T DeserializeContract<T>(string contract)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var ms = new MemoryStream(Encoding.Default.GetBytes(contract)))
            {
                return (T)serializer.ReadObject(ms);
            }
        }

        private static string SerializeJson<T>(T obj)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                return Encoding.Default.GetString(ms.ToArray());
            }
        }

        private static T DeserializeJson<T>(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var ms = new MemoryStream(Encoding.Default.GetBytes(json)))
            {
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}