using Alcoa.Entity.Entity;
using Alcoa.Framework.Common.Entity;
using Alcoa.Framework.Contract.DTOs;
using Alcoa.Framework.Contract.DTOs.Sso;
using System.Collections.Generic;
using System.ServiceModel;

namespace Alcoa.Framework.Contract.Interfaces
{
    [ServiceContract]
    [ServiceKnownType("GetServiceTypes", typeof(ServiceKnownTypesAttribute))]
    public interface ISsoService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        UserDTO GetUser(UserFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        List<UserDTO> SearchUsers(UserListFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        ActiveDirectoryGroupDTO GetActiveDirectoryGroup(ActiveDirectoryGroupFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        List<ActiveDirectoryGroupDTO> SearchActiveDirectoryGroups(ActiveDirectoryGroupListFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        List<NetworkDomainDTO> GetActiveDirectoryDomainList();

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        ApplicationDTO GetApplication(ApplicationFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        List<ApplicationDTO> GetApplicationsList(ApplicationListFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        SsoAuthorizationDTO GetApplicationAuthorizations(ApplicationFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        SsoAuthorizationDTO ValidateAndGetUserAuthorizations(SsoAuthenticationDTO sso);
    }
}