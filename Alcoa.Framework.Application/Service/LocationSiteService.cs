using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Common;
using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Exceptions;
using Alcoa.Framework.Contract.DTOs;
using Alcoa.Framework.Contract.DTOs.LocationSite;
using Alcoa.Framework.Contract.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Alcoa.Framework.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;

namespace Alcoa.Framework.Application.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class LocationSiteService : AbstractService, ILocationSiteService, IDisposable
    {
        private IRepository<Worker> _workerRepo;
        private IRepository<Lbc> _lbcRepo;
        private IRepository<Dept> _deptRepo;
        private IRepository<SsoProfile> _ssoProfileRepo;
        private IRepository<SsoApplication> _ssoApplicationRepo;

        public LocationSiteService()
        {
            try
            {
                MultipleProfiles = new Dictionary<CommonDatabase, string>();
                MultipleProfiles.Add(CommonDatabase.DBALCAP, "FMW");
                MultipleProfiles.Add(CommonDatabase.DBALUAP, "FMWALUA");
                MultipleProfiles.Add(CommonDatabase.DBITPAP, "FMWITPA");
                MultipleProfiles.Add(CommonDatabase.DBITPBP, "FMWITPB");
                MultipleProfiles.Add(CommonDatabase.DBPOCAP, "FMWPOCA");
                MultipleProfiles.Add(CommonDatabase.DBTUBBP, "FMWTUBB");
                MultipleProfiles.Add(CommonDatabase.DBUTGAP, "FMWUTGA");
                MultipleProfiles.Add(CommonDatabase.DBUTGBP, "FMWUTGB");
                MultipleProfiles.Add(CommonDatabase.P300, "FMW300");
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }
        }

        private void InitializeUnitOfWork(CommonDatabase selectedDatabase)
        {
            if (selectedDatabase == CommonDatabase.NONE)
                throw new ServiceException(CommonExceptionType.ValidationException, "Invalid database name");

            //Gets vault profile code 
            var profileName = MultipleProfiles[selectedDatabase];

            //Initializes the Unit of Work connection and repositories
            Uow = new UnitOfWork<LocationSiteContextFmw>(profileName);
            _workerRepo = Uow.GetRepository<Worker>();
            _lbcRepo = Uow.GetRepository<Lbc>();
            _deptRepo = Uow.GetRepository<Dept>();
            _ssoProfileRepo = Uow.GetRepository<SsoProfile>();
            _ssoApplicationRepo = Uow.GetRepository<SsoApplication>();
        }

        public WorkerDTO GetWorker(WorkerFilterDTO filter)
        {
            var dto = default(WorkerDTO);

            try
            {
                var exprList = new List<Expression<Func<Worker, bool>>>();

                if (!string.IsNullOrEmpty(filter.Id))
                    exprList.Add(wo => wo.Id.ToUpper() == filter.Id.ToUpper());

                if (!string.IsNullOrEmpty(filter.Login))
                    exprList.Add(wo => wo.Login.ToUpper() == filter.Login.ToUpper());

                if (!string.IsNullOrEmpty(filter.NameOrDescription))
                    exprList.Add(wo => wo.NameOrDescription.ToUpper() == filter.NameOrDescription.ToUpper());

                if (exprList.Count <= default(int))
                    throw new ServiceException(CommonExceptionType.ParameterException, "Id, Login or NameOrDescription");

                if (string.IsNullOrEmpty(filter.SpecificDatabase.ToString()))
                    throw new ServiceException(CommonExceptionType.ParameterException, "Specific Database");

                InitializeUnitOfWork(filter.SpecificDatabase);

                var worker = _workerRepo.Get(exprList.BuildExpression());

                //Validate worker basic data
                worker.Validate();
                if (worker.Validation.HasErrors)
                    throw new ServiceException(worker.Validation.Results);

                //Get LBC and Dept to create Budget code
                var lbc = _lbcRepo.Get(l => l.LbcId == worker.LbcId);
                var dept = _deptRepo.Get(d => d.DeptId == worker.DeptId);

                worker.BudgetCode = new BudgetCode
                {
                    LbcId = lbc.LbcId,
                    Lbc = lbc,
                    DeptId = dept.DeptId,
                    Dept = dept,
                };

                dto = worker.Map<Worker, WorkerDTO>();
                dto.SourceDatabase = filter.SpecificDatabase.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dto;
        }

        public List<WorkerDTO> GetWorkersList(WorkerListFilterDTO filter)
        {
            var dtos = new List<WorkerDTO>();

            try
            {
                var exprList = new List<Expression<Func<Worker, bool>>>();

                if (!string.IsNullOrEmpty(filter.Status))
                    exprList.Add(wo => wo.Status.ToUpper() == filter.Status.ToUpper());

                if (filter.Ids != null && filter.Ids.Exists(id => !string.IsNullOrEmpty(id)))
                {
                    //Equalizes Ids to Uppercase to search in database
                    filter.Ids = filter.Ids.Where(id => !string.IsNullOrEmpty(id)).Select(id => id.ToUpper()).ToList();
                    exprList.Add(wo => filter.Ids.Contains(wo.Id.ToUpper()));
                }

                if (filter.Logins != null && filter.Logins.Exists(lo => !string.IsNullOrEmpty(lo)))
                {
                    //Equalizes logins in Uppercase to search in database
                    filter.Logins = filter.Logins.Where(lo => !string.IsNullOrEmpty(lo)).Select(lo => lo.ToUpper()).ToList();
                    exprList.Add(wo => filter.Logins.Contains(wo.Login.ToUpper()));
                }

                if (exprList.Count <= default(int))
                    throw new ServiceException(CommonExceptionType.ParameterException, "Logins, Ids or Status");

                if (string.IsNullOrEmpty(filter.SpecificDatabase.ToString()))
                    throw new ServiceException(CommonExceptionType.ParameterException, "Specific Database");

                InitializeUnitOfWork(filter.SpecificDatabase);

                //Gets workers, maps to dto type
                var workersDtos = _workerRepo.SelectWhere(exprList.BuildExpression()).ToList().Map<List<Worker>, List<WorkerDTO>>();

                var lbcIds = workersDtos.Select(wd => wd.LbcId);
                var lbcs = _lbcRepo.SelectWhere(lr => lbcIds.Contains(lr.LbcId)).ToList().Map<List<Lbc>, List<Alcoa.Framework.Contract.DTOs.Corporate.LbcDTO>>();

                var deptIds = workersDtos.Select(wd => wd.DeptId);
                var depts = _deptRepo.SelectWhere(dr => deptIds.Contains(dr.DeptId)).ToList().Map<List<Dept>, List<Alcoa.Framework.Contract.DTOs.Corporate.DeptDTO>>();

                foreach (var worker in workersDtos)
                {
                    //Set database source in returned object
                    worker.SourceDatabase = filter.SpecificDatabase.ToString();

                    //If budget code load option is true
                    if (filter.LoadBudgetCode)
                    {
                        worker.BudgetCode = new Alcoa.Framework.Contract.DTOs.Corporate.BudgetCodeDTO
                        {
                            Lbc = lbcs.FirstOrDefault(l => l.LbcId == worker.LbcId),
                            Dept = depts.FirstOrDefault(d => d.DeptId == worker.DeptId),
                        };
                    }
                }

                dtos.AddRange(workersDtos);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dtos;
        }

        public ApplicationDTO GetApplication(ApplicationFilterDTO filter)
        {
            var dto = default(ApplicationDTO);

            try
            {
                if (string.IsNullOrEmpty(filter.ApplicationCode))
                    throw new ServiceException(CommonExceptionType.ParameterException, "ApplicationCode");

                if (string.IsNullOrEmpty(filter.SpecificDatabase.ToString()))
                    throw new ServiceException(CommonExceptionType.ParameterException, "Specific Database");

                InitializeUnitOfWork(filter.SpecificDatabase);

                //Select application and its profiles
                var ssoApp = GetSsoApplication(filter);

                //Initialize the DTO to return
                dto = ssoApp.Map<SsoApplication, ApplicationDTO>();

                //Set profiles, services and groups names
                foreach (var pro in ssoApp.Profiles)
                {
                    var workerIds = pro.ProfilesAndWorkers.Select(paw => paw.WorkerOrEmployeeId);
                    var workers = _workerRepo.SelectWhere(w => workerIds.Contains(w.Id)).ToList();

                    //Selects profile to update some properties
                    var selectedProfile = dto.Profiles.First(p => p.Id == pro.Id);
                    selectedProfile.SourceDatabase = filter.SpecificDatabase.ToString();

                    //Copy worker list to the specified profile inside Dto
                    selectedProfile.Users =
                        pro.ProfilesAndWorkers
                            .Select(paw => new Alcoa.Framework.Contract.DTOs.Sso.SsoUserDTO
                            {
                                Id = paw.WorkerOrEmployeeId,
                                NameOrDescription = workers.FirstOrDefault(w => w.Id == paw.WorkerOrEmployeeId).Login,
                            })
                            .ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dto;
        }

        private SsoApplication GetSsoApplication(ApplicationFilterDTO filter)
        {
            //Select application
            var ssoApp = _ssoApplicationRepo.Get(app =>
                app.Id.ToLower() == filter.ApplicationCode.ToLower() ||
                app.Mnemonic.ToLower() == filter.ApplicationCode.ToLower());

            ssoApp.Validate();
            if (ssoApp.Validation.HasErrors)
                throw new ServiceException(ssoApp.Validation.Results);

            var query = new List<Expression<Func<SsoProfile, bool>>>();
            if (!string.IsNullOrEmpty(filter.ApplicationCode))
                query.Add(sp => sp.ApplicationId.ToLower() == filter.ApplicationCode.ToLower());

            //Load options
            var loadOptions = new List<string>();
            if (filter.LoadUsersProfiles)
                loadOptions.Add("ProfilesAndWorkers");

            //Gets profile and workers
            ssoApp.Profiles = _ssoProfileRepo.SelectWhere(query.BuildExpression(), loadOptions.ToArray()).ToList();

            //Validates profile basic properties and adds to validation list
            var validations = new List<ValidationResult>();
            ssoApp.Profiles.AsParallel().ForAll(po => { po.Validate(); validations.AddRange(po.Validation.Results); });

            //Throw validation messages
            if (validations.Any())
                throw new ServiceException(validations);

            return ssoApp;
        }

        public void Dispose()
        {
            _workerRepo = null;
            _lbcRepo = null;
            _deptRepo = null;
            _ssoApplicationRepo = null;
            _ssoProfileRepo = null;

            if (Uow != null)
            {
                Uow.Dispose();
                Uow = null;
            }
        }
    }
}