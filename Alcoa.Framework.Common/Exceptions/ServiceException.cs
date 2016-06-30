using Alcoa.Entity.Entity;
using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Alcoa.Framework.Common.Exceptions
{
    /// <summary>
    /// Class to be throwed when a service has errors
    /// </summary>
    public class ServiceException : SystemException
    {
        private readonly ServiceFaultDetail _serviceDetail;
        private readonly string _message = Resources.ResourceManager.GetString(CommonExceptionType.UnknowException.ToString());

        /// <summary>
        /// Accepts a validation list to be formated and sent back to consumer
        /// </summary>
        public ServiceException(IEnumerable<ValidationResult> validationList)
        {
            _serviceDetail = new ServiceFaultDetail(validationList.Select(vl => vl.ErrorMessage));

            throw new FaultException<ServiceFaultDetail>(_serviceDetail, new FaultReason(CommonExceptionType.ValidationException.ToString()));
        }

        /// <summary>
        /// Accepts the common exception type enum and bound with the thrwoed exception
        /// </summary>
        public ServiceException(CommonExceptionType exType, Exception ex)
        {
            _message = Resources.ResourceManager.GetString(exType.ToString()) + Environment.NewLine + " (" + ex.GetInnerExceptionMessage() + ")";
            _serviceDetail = new ServiceFaultDetail();
            _serviceDetail.Add(_message);

            if (ex.InnerException != null)
                throw new FaultException<ServiceFaultDetail>(_serviceDetail, new FaultReason(exType.ToString()), new FaultCode(ex.InnerException.Source));

            throw new FaultException<ServiceFaultDetail>(_serviceDetail, new FaultReason(exType.ToString()));
        }

        /// <summary>
        /// Accepts the common exception type enum and bound with extra fields
        /// </summary>
        public ServiceException(CommonExceptionType exType, params object[] fields)
        {
            _message = Resources.ResourceManager.GetString(exType.ToString()) ?? _message;
            _message = string.Format(_message, fields);
            _serviceDetail = new ServiceFaultDetail();
            _serviceDetail.Add(_message);

            throw new FaultException<ServiceFaultDetail>(_serviceDetail, new FaultReason(exType.ToString()));
        }
    }
}