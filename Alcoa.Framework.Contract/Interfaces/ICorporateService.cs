using Alcoa.Entity.Entity;
using Alcoa.Framework.Common.Entity;
using Alcoa.Framework.Contract.DTOs;
using Alcoa.Framework.Contract.DTOs.Corporate;
using System.Collections.Generic;
using System.ServiceModel;

namespace Alcoa.Framework.Contract.Interfaces
{
    [ServiceContract]
    [ServiceKnownType("GetServiceTypes", typeof(ServiceKnownTypesAttribute))]
    public interface ICorporateService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        WorkerDTO GetWorker(WorkerFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        List<WorkerDTO> GetWorkersList(WorkerListFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        List<ApplicationDTO> GetApplicationsList(Alcoa.Framework.Contract.DTOs.Corporate.ApplicationListFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        AreaDTO GetArea(AreaFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        List<AreaDTO> GetAreaList(AreaListFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        SiteDTO GetSite(SiteFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        List<SiteDTO> GetSiteList(SiteListFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        BudgetCodeDTO GetBudgetCode(BudgetCodeFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        List<BudgetCodeDTO> GetBudgetCodeList(BudgetCodeListFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        string GetEncryptedConnectionString(ConnectionStringFilterDTO filter);
    }
}