using Alcoa.Entity.Entity;
using Alcoa.Framework.Common.Entity;
using Alcoa.Framework.Contract.DTOs;
using System.ServiceModel;

namespace Alcoa.Framework.Contract.Interfaces
{
    [ServiceContract]
    [ServiceKnownType("GetServiceTypes", typeof(ServiceKnownTypesAttribute))]
    public interface IEmailService
    {
    }
}