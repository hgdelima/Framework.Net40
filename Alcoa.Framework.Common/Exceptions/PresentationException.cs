using Alcoa.Entity.Entity;
using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Properties;
using System;
using System.ServiceModel;

namespace Alcoa.Framework.Common.Exceptions
{
    /// <summary>
    /// Class to be throwed when a presentation site got errors
    /// </summary>
    public class PresentationException : BaseException
    {
        /// <summary>
        /// 
        /// </summary>
        public PresentationException(string message)
            : base(message)
        {
            throw new ApplicationException(message);
        }

        /// <summary>
        /// 
        /// </summary>
        public PresentationException(CommonExceptionType errorType, params object[] fields)
            : base(errorType, fields)
        {
            throw new ApplicationException(_message);
        }

        /// <summary>
        /// 
        /// </summary>
        public PresentationException(CommonExceptionType errorType, Exception ex) 
            : base(errorType, ex)
        {
            if (ex.GetType() == typeof(CommunicationException))
                _message += Environment.NewLine + Resources.ResourceManager.GetString(CommonExceptionType.SerializationException.ToString());

            if (ex.GetType() == typeof(FaultException<ServiceFaultDetail>))
            {
                var exceptionDetail = ((FaultException<ServiceFaultDetail>)ex).Detail;

                if (exceptionDetail != null)
                    _message += Environment.NewLine + exceptionDetail.Message;
            }
            else
                _message += Environment.NewLine + ex.Message;

            throw new ApplicationException(_message);
        }
    }
}
