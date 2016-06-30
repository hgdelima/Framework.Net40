using Alcoa.Entity.Entity;
using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Properties;
using System;
using System.ServiceModel;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps manipulate Exception formats
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// Gets all exception message levels
        /// </summary>
        public static string GetAllExceptionMessages(this Exception ex)
        {
            var message = ex.Message;

            //Checks for WCF fault exceptions
            if (ex is FaultException<ServiceFaultDetail>)
                message += Environment.NewLine + ((FaultException<ServiceFaultDetail>)ex).Detail.Message;

            else if (ex is FaultException)
                message += Environment.NewLine + ((FaultException)ex).Message;

            if (ex.InnerException == null)
                return message;
            else
                return message += Environment.NewLine + ex.InnerException.GetAllExceptionMessages();
        }

        /// <summary>
        /// Gets last inner exception message
        /// </summary>
        public static string GetInnerExceptionMessage(this Exception ex)
        {
            var message = ex.Message;

            if (ex.InnerException == null)
                return message;
            else
                return message = ex.InnerException.GetInnerExceptionMessage();
        }

        /// <summary>
        /// Get exception messages as string based on CommonExceptionType enum
        /// </summary>
        public static string GetStringMessage(this CommonExceptionType exceptionType)
        {
            return Resources.ResourceManager.GetString(exceptionType.ToString()) ?? string.Empty;
        }

        /// <summary>
        /// Converts a string to Layer enumerable
        /// </summary>
        internal static Layer ToLayer(this string layer)
        {
            var layerType = default(Layer);

            if (!string.IsNullOrEmpty(layer))
                Enum.TryParse(layer, true, out layerType);

            return layerType;
        }

        /// <summary>
        /// Converts a string to LogLevel enumerable
        /// </summary>
        internal static LogLevel ToLevel(this string level)
        {
            var levelType = LogLevel.Off;

            if (!string.IsNullOrEmpty(level))
                Enum.TryParse(level, true, out levelType);

            return levelType;
        }
    }
}