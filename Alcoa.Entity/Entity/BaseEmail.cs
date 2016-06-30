using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alcoa.Entity.Entity
{
    /// <summary>
    /// Base class with e-mail data that can be reused by many objects
    /// </summary>
    [DataContract(Namespace = "Alcoa.Entity")]
    public class BaseEmail : Base
    {
        public BaseEmail()
        {
            From = string.Empty;
            FromName = string.Empty;
            To = string.Empty;
            Subject = string.Empty;
            Body = string.Empty;
            SentDateTime = DateTime.Now;
            IsHtml = false;
            Attachments = new List<BaseEmailAttachment>();
        }

        [DataMember]
        public string From { get; set; }

        [DataMember]
        public string FromName { get; set; }

        [DataMember]
        public string To { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public DateTime? SentDateTime { get; set; }

        [DataMember]
        public bool IsHtml { get; set; }

        [DataMember]
        public List<BaseEmailAttachment> Attachments { get; set; }
    }
}