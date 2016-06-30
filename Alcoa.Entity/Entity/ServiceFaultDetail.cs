using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Entity.Entity
{
    /// <summary>
    /// Class to be used in service throw messages
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "Alcoa.Entity")]
    public class ServiceFaultDetail
    {
        /// <summary>
        /// List to hold service messages to be returned
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Initializes a empty list to return service messages
        /// </summary>
        public ServiceFaultDetail()
        {
            Message = string.Empty;
        }

        /// <summary>
        /// Initializes a list with other messages to return service messages
        /// </summary>
        public ServiceFaultDetail(IEnumerable<string> messageList)
        {
            Message = string.Join(Environment.NewLine, messageList);
        }

        /// <summary>
        /// Adds service messages to be returned
        /// </summary>
        public void Add(string message)
        {
            Message += Environment.NewLine + message;
        }
    }
}