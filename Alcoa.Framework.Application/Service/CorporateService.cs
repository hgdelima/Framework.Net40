using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Common;
using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Exceptions;
using Alcoa.Framework.Contract.DTOs;
using Alcoa.Framework.Contract.DTOs.Corporate;
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
    public class CorporateService : AbstractService, ICorporateService, IDisposable
    {
        private IRepository<Worker> _workerRepo;
        private IRepository<Area> _areaRepo;
        private IRepository<Site> _siteRepo;
        private IRepository<BudgetCode> _budgetRepo;
        private IRepository<SsoApplication> _appRepo;
        private IActiveDirectoryRepository _adRepo;

        public CorporateService()
        {
            try
            {
                //Initializes the Unit of Work connection and repositories
                Uow = new UnitOfWork<CorporateContextFmw>();

                _adRepo = Uow.GetRepositoryActiveDirectory();
                _workerRepo = Uow.GetRepository<Worker>();
                _areaRepo = Uow.GetRepository<Area>();
                _siteRepo = Uow.GetRepository<Site>();
                _budgetRepo = Uow.GetRepository<BudgetCode>();
                _appRepo = Uow.GetRepository<SsoApplication>();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }
        }

        public WorkerDTO GetWorker(WorkerFilterDTO filter)
        {
            var dto = default(WorkerDTO);

            try
            {
                var exprList = new List<Expression<Func<Worker, bool>>>();

                if (!string.IsNullOrEmpty(filter.Id))
                    exprList.Add(wo => wo.Id.ToLower() == filter.Id.ToLower());

                if (!string.IsNullOrEmpty(filter.Login))
                    exprList.Add(wo => wo.Login.ToLower() == filter.Login.ToLower());

                if (!string.IsNullOrEmpty(filter.NameOrDescription))
                    exprList.Add(wo => wo.NameOrDescription.ToLower() == filter.NameOrDescription.ToLower());

                if (exprList.Count <= default(int))
                    throw new ServiceException(CommonExceptionType.ParameterException, "Id, Login or NameOrDescription");

                var loadOptions = new List<string>
                {
                    "BudgetCode",
                    "BudgetCode.Area",
                    "BudgetCode.Site",
                    "BudgetCode.Dept",
                    "BudgetCode.Lbc"
                };

                if (filter.LoadManager)
                {
                    loadOptions.Add("Manager");
                    loadOptions.Add("Manager.Manager");
                }

                if (filter.LoadEmployees)
                {
                    loadOptions.Add("Employees");
                    loadOptions.Add("Employees.Employee");
                }

                //Executes query
                var worker = _workerRepo.Get(exprList.BuildExpression(), loadOptions.ToArray());

                //Validate worker basic data
                worker.Validate();

                if (worker.Validation.HasErrors)
                    throw new ServiceException(worker.Validation.Results);

                dto = worker.Map<Worker, WorkerDTO>();
                dto.SourceDatabase = CommonDatabase.DBALCAP.ToString();

                //Map employees data
                if (worker.Employees != null && worker.Employees.Count > 0)
                {
                    var employees = worker.Employees.Select(e => e.Employee).ToList();
                    dto.Employees = employees.Map<List<Worker>, List<WorkerLeafDTO>>();
                }

                // Map third partners' data
                if (filter.LoadManager || filter.LoadThirdPartners)
                {
                    var adUser = _adRepo.GetUser(dto.Login, filter.LoadManager, filter.LoadThirdPartners);

                    //Map manager data
                    if (worker.Manager != null && worker.Manager.FirstOrDefault() != null)
                    {
                        var manager = worker.Manager.FirstOrDefault().Manager;
                        dto.Manager = manager.Map<Worker, WorkerLeafDTO>();
                    }
                    else if (adUser.ActiveDirectoryManager.Login != null)
                    {
                        var manager = _workerRepo.Get(w => w.Login.ToUpper() == adUser.ActiveDirectoryManager.Login.ToUpper());
                        dto.Manager = manager.Map<Worker, WorkerLeafDTO>();
                    }

                    if (filter.LoadThirdPartners)
                    {
                        var logins = adUser.ActiveDirectoryThirdPartners.Select(ad => ad.Login.ToUpper()).Distinct().ToList();
                        var thirdPartners = _workerRepo.SelectWhere(w => logins.Contains(w.Login.ToUpper())).ToList();
                        dto.ThirdPartners = thirdPartners.Map<List<Worker>, List<ThirdPartnerDTO>>();
                    }
                }
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
                    exprList.Add(wo => wo.Status.ToLower() == filter.Status.ToLower());

                if (filter.Ids != null && filter.Ids.Exists(id => !string.IsNullOrEmpty(id)))
                {
                    //Equalizes Ids to Uppercase to search in database
                    filter.Ids = filter.Ids.Where(id => !string.IsNullOrEmpty(id)).Select(id => id.ToLower()).ToList();
                    exprList.Add(wo => filter.Ids.Contains(wo.Id.ToLower()));
                }

                if (filter.Logins != null && filter.Logins.Exists(lo => !string.IsNullOrEmpty(lo)))
                {
                    //Equalizes logins in Uppercase to search in database
                    filter.Logins = filter.Logins.Where(lo => !string.IsNullOrEmpty(lo)).Select(lo => lo.ToLower()).ToList();
                    exprList.Add(wo => filter.Logins.Contains(wo.Login.ToLower()));
                }

                if (exprList.Count <= default(int))
                    throw new ServiceException(CommonExceptionType.ParameterException, "Logins, Ids or Status");

                var loadOptions = new List<string>();
                if (filter.LoadManager)
                {
                    loadOptions.Add("Manager");
                    loadOptions.Add("Manager.Manager");
                }

                if (filter.LoadEmployees)
                {
                    loadOptions.Add("Employees");
                    loadOptions.Add("Employees.Employee");
                }

                if (filter.LoadBudgetCode)
                {
                    loadOptions.Add("BudgetCode");
                    loadOptions.Add("BudgetCode.Area");
                    loadOptions.Add("BudgetCode.Site");
                    loadOptions.Add("BudgetCode.Dept");
                    loadOptions.Add("BudgetCode.Lbc");
                }

                //Get workers using query options
                var workers = _workerRepo.SelectWhere(exprList.BuildExpression(), loadOptions.ToArray()).ToList();

                //Validates all basic worker properties
                var validations = new List<ValidationResult>();
                workers.AsParallel().ForAll(co => { co.Validate(); validations.AddRange(co.Validation.Results); });

                if (validations.Any())
                    throw new ServiceException(validations);

                dtos = workers.Map<List<Worker>, List<WorkerDTO>>();

                //Maps manager and employees data
                workers.AsParallel().ForAll(wo =>
                {
                    var dto = dtos.FirstOrDefault(d => d.Id.ToUpper() == wo.Id.ToUpper());
                    dto.SourceDatabase = CommonDatabase.DBALCAP.ToString();

                    if (wo.Employees != null && wo.Employees.Count > 0)
                    {
                        var employees = wo.Employees.Select(e => e.Employee).ToList();
                        dto.Employees = employees.Map<List<Worker>, List<WorkerLeafDTO>>();
                    }

                    //Maps managers and third partners data
                    if (filter.LoadManager || filter.LoadThirdPartners)
                    {
                        var adUser = _adRepo.GetUser(dto.Login, filter.LoadManager, filter.LoadThirdPartners);

                        //Map manager data
                        if (wo.Manager != null && wo.Manager.FirstOrDefault() != null)
                        {
                            var manager = wo.Manager.FirstOrDefault().Manager;
                            dto.Manager = manager.Map<Worker, WorkerLeafDTO>();
                        }
                        else if (adUser.ActiveDirectoryManager.Login != null)
                        {
                            var manager = _workerRepo.Get(w => w.Login.ToUpper() == adUser.ActiveDirectoryManager.Login.ToUpper());
                            dto.Manager = manager.Map<Worker, WorkerLeafDTO>();
                        }

                        if (filter.LoadThirdPartners)
                        {
                            var logins = adUser.ActiveDirectoryThirdPartners.Select(ad => ad.Login.ToUpper()).Distinct().ToList();
                            var thirdPartners = _workerRepo.SelectWhere(w => logins.Contains(w.Login.ToUpper())).ToList();
                            dto.ThirdPartners = thirdPartners.Map<List<Worker>, List<ThirdPartnerDTO>>();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dtos;
        }

        public List<ApplicationDTO> GetApplicationsList(ApplicationListFilterDTO filter)
        {
            var dtos = new List<ApplicationDTO>();

            try
            {
                var isInactive = (filter.IsActive == true) ? "N" : "S";

                //Select application and its profiles
                var apps = _appRepo.SelectWhere(app => app.IsInactive.ToUpper() == isInactive).ToList();

                var validations = new List<ValidationResult>();
                apps.AsParallel().ForAll(a => { a.Validate(); validations.AddRange(a.Validation.Results); });

                if (validations.Any())
                    throw new ServiceException(validations);

                //Initialize the DTO to return
                dtos = apps.Map<List<SsoApplication>, List<ApplicationDTO>>();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dtos;
        }

        public AreaDTO GetArea(AreaFilterDTO filter)
        {
            var dto = default(AreaDTO);

            try
            {
                if (string.IsNullOrEmpty(filter.SiteId) || string.IsNullOrEmpty(filter.AreaId))
                    throw new ServiceException(CommonExceptionType.ParameterException, "SiteId or AreaId");

                var area = _areaRepo.Get(a =>
                    a.SiteId.ToLower() == filter.SiteId.ToLower() &&
                    a.AreaId.ToLower() == filter.AreaId.ToLower(),
                    "AreaParent", "SubAreas", "Site", "BudgetCodes", "Manager");

                area.Validate();

                if (area.Validation.HasErrors)
                    throw new ServiceException(area.Validation.Results);

                dto = area.Map<Area, AreaDTO>();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dto;
        }

        public List<AreaDTO> GetAreaList(AreaListFilterDTO filter)
        {
            var dtos = new List<AreaDTO>();

            try
            {
                var exprList = new List<Expression<Func<Area, bool>>>();

                if (!string.IsNullOrEmpty(filter.IsActive))
                    exprList.Add(al => al.IsActive.ToLower() == filter.IsActive.ToLower());

                if (!string.IsNullOrEmpty(filter.NameOrDescription))
                    exprList.Add(al => al.NameOrDescription.ToLower().Contains(filter.NameOrDescription.ToLower()));

                if (exprList.Count <= default(int))
                    throw new ServiceException(CommonExceptionType.ParameterException, "IsActive or NameOrDescription");

                var loadOptions = new List<string>();
                if (filter.LoadAreaParent)
                    loadOptions.Add("AreaParent");

                if (filter.LoadSubAreas)
                    loadOptions.Add("SubAreas");

                if (filter.LoadSite)
                    loadOptions.Add("Site");

                if (filter.LoadBudgetCodes)
                    loadOptions.Add("BudgetCodes");

                if (filter.LoadManager)
                    loadOptions.Add("Manager");

                //Get area using query options
                var areaList = _areaRepo.SelectWhere(exprList.BuildExpression(), loadOptions.ToArray()).ToList();

                var validations = new List<ValidationResult>();
                areaList.AsParallel().ForAll(a => { a.Validate(); validations.AddRange(a.Validation.Results); });

                if (validations.Any())
                    throw new ServiceException(validations);

                dtos = areaList.Map<List<Area>, List<AreaDTO>>();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dtos;
        }

        public SiteDTO GetSite(SiteFilterDTO filter)
        {
            var dto = default(SiteDTO);

            try
            {
                if (string.IsNullOrEmpty(filter.Id))
                    throw new ServiceException(CommonExceptionType.ParameterException, "Id");

                var site = _siteRepo.Get(si => si.Id.ToLower() == filter.Id.ToLower(), "Lbcs", "Areas");

                site.Validate();

                if (site.Validation.HasErrors)
                    throw new ServiceException(site.Validation.Results);

                dto = site.Map<Site, SiteDTO>();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dto;
        }

        public List<SiteDTO> GetSiteList(SiteListFilterDTO filter)
        {
            var dtos = new List<SiteDTO>();

            try
            {
                var exprList = new List<Expression<Func<Site, bool>>>();

                if (!string.IsNullOrEmpty(filter.IsActive))
                    exprList.Add(si => si.IsActive.ToLower() == filter.IsActive.ToLower());

                if (!string.IsNullOrEmpty(filter.NameOrDescription))
                    exprList.Add(si => si.NameOrDescription.ToLower().Contains(filter.NameOrDescription.ToLower()));

                if (exprList.Count <= default(int))
                    throw new ServiceException(CommonExceptionType.ParameterException, "IsActive or NameOrDescription");

                var loadOptions = new List<string>();
                if (filter.LoadLbcs)
                    loadOptions.Add("Lbcs");

                if (filter.LoadAreas)
                    loadOptions.Add("Areas");

                //Get sites using query options
                var siteList = _siteRepo.SelectWhere(exprList.BuildExpression(), loadOptions.ToArray()).ToList();

                var validations = new List<ValidationResult>();
                siteList.AsParallel().ForAll(si => { si.Validate(); validations.AddRange(si.Validation.Results); });

                if (validations.Any())
                    throw new ServiceException(validations);

                dtos = siteList.Map<List<Site>, List<SiteDTO>>();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dtos;
        }

        public BudgetCodeDTO GetBudgetCode(BudgetCodeFilterDTO filter)
        {
            var dto = default(BudgetCodeDTO);

            try
            {
                if (string.IsNullOrEmpty(filter.SiteId) ||
                    string.IsNullOrEmpty(filter.AreaId) ||
                    string.IsNullOrEmpty(filter.DeptId) ||
                    string.IsNullOrEmpty(filter.LbcId))
                    throw new ServiceException(CommonExceptionType.ParameterException, "SiteId or AreaId or DeptId or LbcId");

                var budgetCode = _budgetRepo.Get(bu =>
                    bu.SiteId.ToLower() == filter.SiteId.ToLower() &&
                    bu.AreaId.ToLower() == filter.AreaId.ToLower() &&
                    bu.DeptId.ToLower() == filter.DeptId.ToLower() &&
                    bu.LbcId.ToLower() == filter.LbcId.ToLower(),
                    "Site", "Area", "Dept", "Lbc", "Manager");

                budgetCode.Validate();

                if (budgetCode.Validation.HasErrors)
                    throw new ServiceException(budgetCode.Validation.Results);

                dto = budgetCode.Map<BudgetCode, BudgetCodeDTO>();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dto;
        }

        public List<BudgetCodeDTO> GetBudgetCodeList(BudgetCodeListFilterDTO filter)
        {
            var dtos = new List<BudgetCodeDTO>();

            try
            {
                var exprList = new List<Expression<Func<BudgetCode, bool>>>();

                if (!string.IsNullOrEmpty(filter.IsActive))
                    exprList.Add(bu => bu.IsActive.ToLower() == filter.IsActive.ToLower());

                if (!string.IsNullOrEmpty(filter.SiteId))
                    exprList.Add(bu => bu.SiteId.ToLower() == filter.SiteId.ToLower());

                if (!string.IsNullOrEmpty(filter.AreaId))
                    exprList.Add(bu => bu.AreaId.ToLower() == filter.AreaId.ToLower());

                if (!string.IsNullOrEmpty(filter.DeptId))
                    exprList.Add(bu => bu.DeptId.ToLower() == filter.DeptId.ToLower());

                if (!string.IsNullOrEmpty(filter.LbcId))
                    exprList.Add(bu => bu.LbcId.ToLower() == filter.LbcId.ToLower());

                if (exprList.Count <= default(int))
                    throw new ServiceException(CommonExceptionType.ParameterException, "IsActive or SiteId or AreaId or DeptId or LbcId");

                var loadOptions = new List<string>();
                if (filter.LoadSite)
                    loadOptions.Add("Site");

                if (filter.LoadArea)
                    loadOptions.Add("Area");

                if (filter.LoadDept)
                    loadOptions.Add("Dept");

                if (filter.LoadLbc)
                    loadOptions.Add("Lbc");

                if (filter.LoadManager)
                    loadOptions.Add("Manager");

                //Get budgetCodes using query options
                var budgetList = _budgetRepo.SelectWhere(exprList.BuildExpression(), loadOptions.ToArray()).ToList();

                var validations = new List<ValidationResult>();
                budgetList.AsParallel().ForAll(bu => { bu.Validate(); validations.AddRange(bu.Validation.Results); });

                if (validations.Any())
                    throw new ServiceException(validations);

                dtos = budgetList.Map<List<BudgetCode>, List<BudgetCodeDTO>>();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dtos;
        }

        public string GetEncryptedConnectionString(ConnectionStringFilterDTO filter)
        {
            var connectionString = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(filter.ConnectionString))
                    throw new ServiceException(CommonExceptionType.ParameterException, "ConnectionString");

                var prefix = CommonResource.GetString("PassNumbers") + CommonResource.GetString("PassSpecialChars");
                var pass = prefix + CommonResource.GetString("PassText") + prefix;

                var upperConnection = filter.ConnectionString.ToUpper();
                if (upperConnection.Contains("DATA SOURCE") &&
                    upperConnection.Contains("USER ID") &&
                    upperConnection.Contains("PASSWORD"))
                    connectionString = CryptographHelper.RijndaelEncrypt(filter.ConnectionString, pass);

                else if (upperConnection.Contains("SERVER") &&
                    upperConnection.Contains("DATABASE") &&
                    (upperConnection.Contains("TRUSTED_CONNECTION") ||
                    (upperConnection.Contains("USER ID") && upperConnection.Contains("PASSWORD"))))
                    connectionString = CryptographHelper.RijndaelEncrypt(filter.ConnectionString, pass);

                else if (upperConnection.Contains("DATA SOURCE") &&
                    upperConnection.Contains("PROVIDER"))
                    connectionString = CryptographHelper.RijndaelEncrypt(filter.ConnectionString, pass);
                else
                    throw new ServiceException(CommonExceptionType.ValidationException,
                        "ConnectionString parameter, must follow connection standards " + Environment.NewLine +
                        "For Oracle" + Environment.NewLine +
                        "DATA SOURCE=#########;PERSIST SECURITY INFO=FALSE;USER ID=######;PASSWORD=######;" + Environment.NewLine +
                        "For SQL Server" + Environment.NewLine +
                        "SERVER=###############;DATABASE=###############;[USER ID=######;PASSWORD=######;|TRUSTED_CONNECTION=TRUE;]" + Environment.NewLine +
                        "For MS Access" + Environment.NewLine +
                        "PROVIDER=###############;DATA SOURCE=###############;PERSIST SECURITY INFO=FALSE;[USER ID=######;PASSWORD=######;]");

            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return connectionString;
        }

        public void Dispose()
        {
            _workerRepo = null;
            _areaRepo = null;
            _siteRepo = null;
            _budgetRepo = null;
            _appRepo = null;

            if (Uow != null)
            {
                Uow.Dispose();
                Uow = null;
            }
        }
    }
}