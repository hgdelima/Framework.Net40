using Alcoa.Entity.Entity;
using Alcoa.Framework.Common.Entity;
using Alcoa.Framework.Contract.DTOs;
using Alcoa.Framework.Contract.DTOs.LocationSite;
using System.Collections.Generic;
using System.ServiceModel;

namespace Alcoa.Framework.Contract.Interfaces
{
    [ServiceContract]
    [ServiceKnownType("GetServiceTypes", typeof(ServiceKnownTypesAttribute))]
    public interface ILocationSiteService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        WorkerDTO GetWorker(WorkerFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        List<WorkerDTO> GetWorkersList(WorkerListFilterDTO filter);

        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetail))]
        ApplicationDTO GetApplication(ApplicationFilterDTO filter);
    }
}