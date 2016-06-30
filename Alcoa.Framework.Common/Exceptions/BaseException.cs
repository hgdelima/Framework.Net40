using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Properties;
using System;

namespace Alcoa.Framework.Common.Exceptions
{
    public class BaseException : SystemException
    {
        public string _errorCode { get; set; }
        protected string _message = string.Empty;
        protected readonly Exception _exception;

        /// <summary>
        /// 
        /// </summary>
        public BaseException(string message)
        {
            _message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        public BaseException(CommonExceptionType errorType, params object[] fields)
        {
            _message = Resources.ResourceManager.GetString(errorType.ToString()) ?? _message;
            _message = string.Format(_message, fields);
        }

        /// <summary>
        /// 
        /// </summary>
        public BaseException(CommonExceptionType errorType, Exception ex)
        {
            _message = Resources.ResourceManager.GetString(errorType.ToString()) ?? _message;
            _message = string.Format(_message, string.Empty);

            _exception = ex;
        }
    }
}
