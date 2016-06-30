using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Common;
using Alcoa.Framework.Common.Entity;
using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Exceptions;
using Alcoa.Framework.Contract.DTOs;
using Alcoa.Framework.Contract.DTOs.Sso;
using Alcoa.Framework.Contract.Interfaces;
using Alcoa.Framework.DataAccess;
using Alcoa.Framework.DataAccess.Context.Oracle;
using Alcoa.Framework.Domain.Entity;
using Alcoa.Framework.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Threading;

namespace Alcoa.Framework.Application.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class SsoService : AbstractService, ISsoService, IDisposable
    {
        private IRepository<Worker> _workerRepo;
        private IRepository<SsoApplication> _ssoApplicationRepo;
        private IRepository<SsoProfile> _ssoProfileRepo;
        private IRepository<SsoProfileAndWorker> _ssoProfileWorkerRepo;
        private IRepository<SsoProfileAndService> _ssoProfileServiceRepo;
        private IRepository<ApplicationParameter> _appParameterRepo;
        private IRepository<ApplicationTranslation> _appTranslationRepo;
        private IActiveDirectoryRepository _adRepo;

        public SsoService()
        {
            try
            {
                Uow = new UnitOfWork<SsoContextSso>();
                _adRepo = Uow.GetRepositoryActiveDirectory();
                _workerRepo = Uow.GetRepository<Worker>();
                _ssoApplicationRepo = Uow.GetRepository<SsoApplication>();
                _ssoProfileRepo = Uow.GetRepository<SsoProfile>();
                _ssoProfileServiceRepo = Uow.GetRepository<SsoProfileAndService>();
                _ssoProfileWorkerRepo = Uow.GetRepository<SsoProfileAndWorker>();
                _appParameterRepo = Uow.GetRepository<ApplicationParameter>();
                _appTranslationRepo = Uow.GetRepository<ApplicationTranslation>();

                LanguageCultureName = Thread.CurrentThread.CurrentCulture.Name.ToLower();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }
        }

        public UserDTO GetUser(UserFilterDTO filter)
        {
            var dto = default(UserDTO);

            try
            {
                if (string.IsNullOrEmpty(filter.Login) &&
                    string.IsNullOrEmpty(filter.Id))
                    throw new ServiceException(CommonExceptionType.ParameterException, "UserIdentity or Id");

                //Get Worker domain object and set Login in filter
                var worker = GetWorker(filter);
                filter.Login = worker.Login;

                //Get worker related apps
                worker.Applications = GetUserApplications(filter, new ApplicationFilterDTO
                {
                    LoadTranslations = filter.LoadTranslations,
                    LanguageCultureName = filter.LanguageCultureName
                });

                //Initialize the DTO to return and cast dependent objects
                dto = worker.Map<Worker, UserDTO>();

                if (worker.Applications != null && worker.Applications.Count > 0)
                {
                    //Set profiles, services and groups names
                    foreach (var pro in worker.Applications.SelectMany(app => app.Profiles))
                    {
                        var appId = pro.ApplicationId;
                        var app = dto.Applications.FirstOrDefault(ap => ap.Id == appId);

                        if (app != default(ApplicationDTO))
                        {
                            var proId = pro.Id;
                            var profile = app.Profiles.FirstOrDefault(p => p.Id == proId);

                            if (profile != default(ProfileDTO))
                            {
                                //Inverts the hierarchy between SSOGroup and SSOServices, putting SSOGroup as a container
                                profile.SsoGroups = pro.ProfilesAndServices
                                    .Where(pas => pas.ApplicationId == appId && pas.ProfileId == proId)
                                    .Select(pad => new SsoGroupDTO
                                    {
                                        Id = pad.Service.GroupId,
                                        NameOrDescription = pad.Service.SsoGroup.NameOrDescription,
                                        Services = pro.ProfilesAndServices
                                            .Where(ps => ps.ApplicationId == appId && ps.ProfileId == proId && ps.Service.GroupId == pad.Service.GroupId)
                                            .Select(se => se.Service)
                                            .ToList()
                                            .Map<List<SsoServices>, List<SsoServicesDTO>>(),
                                    })
                                    .Distinct(new EqualBy<SsoGroupDTO>((x, y) => x.Id == y.Id))
                                    .ToList();

                                //Copy AD group list to the specified profile inside Dto
                                profile.ActiveDirectoryGroups = pro.ProfilesAndActiveDirectories
                                    .Where(pad => pad.ApplicationId == appId && pad.ProfileId == proId)
                                    .Select(pad => pad.AdGroup)
                                    .ToList()
                                    .Map<List<ActiveDirectoryGroup>, List<ActiveDirectoryGroupDTO>>();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dto;
        }

        public List<UserDTO> SearchUsers(UserListFilterDTO filter)
        {
            var dto = new List<UserDTO>();

            try
            {
                if (filter.LoginsList == null || filter.LoginsList.Count <= default(int))
                    throw new ServiceException(CommonExceptionType.ParameterException, "UserIdentityList");

                if (string.IsNullOrEmpty(filter.Domain))
                    throw new ServiceException(CommonExceptionType.ParameterException, "Domain");

                var workers = new List<Worker>();
                var workersCorp = new List<Worker>();

                using (var uowCorporate = new UnitOfWork<CorporateContextFmw>())
                {
                    var _workerCorporateRepo = uowCorporate.GetRepository<Worker>();

                    foreach (var userIdentity in filter.LoginsList.Where(ll => !string.IsNullOrEmpty(ll)).ToList())
                    {
                        //Get workers from ActiveDirectory and adds to list
                        var adWorkers = _adRepo.SearchUsers(userIdentity, filter.Domain)
                            .Map<List<BaseActiveDirectoryUser>, List<Worker>>();

                        workers.AddRange(adWorkers);

                        //Get workers from Corporate database and adds to list
                        var corporateWorkers = _workerCorporateRepo.SelectWhere(wo =>
                            wo.Login.ToLower().Contains(userIdentity.ToLower()) ||
                            wo.NameOrDescription.ToLower().Contains(userIdentity.ToLower()));

                        workersCorp.AddRange(corporateWorkers);
                    }
                }

                foreach (var wo in workers)
                {
                    var woCorp = workersCorp.FirstOrDefault(wc => wc.Login == wo.Login);

                    //Maps corporate data only to null properties in Worker object
                    if (woCorp != null)
                    {
                        //Cleans the Id to get Id from Corporate
                        wo.Id = (woCorp.Id ?? wo.Id);
                        wo.BranchLine = (wo.UserExtraInfo.Phone ?? wo.BranchLine);
                        wo.MapOnlyNulls<Worker>(woCorp);
                    }
                }

                //Validates all workers basic properties
                var validations = new List<ValidationResult>();
                workers.AsParallel().ForAll(wo => { wo.Validate(); validations.AddRange(wo.Validation.Results); });

                if (validations.Any())
                    throw new ServiceException(validations);

                //Filter workers by Status
                if (!string.IsNullOrEmpty(filter.Status))
                    workers = workers.Where(wo => wo.Status == filter.Status.Trim()).ToList();

                dto = workers.Map<List<Worker>, List<UserDTO>>();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dto;
        }

        public ActiveDirectoryGroupDTO GetActiveDirectoryGroup(ActiveDirectoryGroupFilterDTO filter)
        {
            var dto = default(ActiveDirectoryGroupDTO);

            try
            {
                //Casts from AD base group to domain AD Group object
                var adGroup = _adRepo.GetGroup(filter.GroupIdentity, true).Map<BaseActiveDirectoryGroup, ActiveDirectoryGroup>();

                adGroup.Validate();

                if (adGroup.Validation.HasErrors)
                    throw new ServiceException(adGroup.Validation.Results);

                dto = adGroup.Map<ActiveDirectoryGroup, ActiveDirectoryGroupDTO>();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dto;
        }

        public List<ActiveDirectoryGroupDTO> SearchActiveDirectoryGroups(ActiveDirectoryGroupListFilterDTO filter)
        {
            var dto = default(List<ActiveDirectoryGroupDTO>);

            try
            {
                if (filter.GroupIdentities == null || filter.GroupIdentities.Count <= default(int))
                    throw new ServiceException(CommonExceptionType.ParameterException, "GroupIdentities");

                if (string.IsNullOrEmpty(filter.Domain))
                    throw new ServiceException(CommonExceptionType.ParameterException, "Domain");

                var adGroupList = new List<ActiveDirectoryGroup>();
                foreach (var identity in filter.GroupIdentities)
                {
                    //Search and casts from groupBase to domain objet Group
                    adGroupList = _adRepo.SearchGroups(identity, filter.Domain)
                        .Map<List<BaseActiveDirectoryGroup>, List<ActiveDirectoryGroup>>();
                }

                var validations = new List<ValidationResult>();
                adGroupList.AsParallel().ForAll(agl => { agl.Validate(); validations.AddRange(agl.Validation.Results); });

                if (validations.Any())
                    throw new ServiceException(validations);

                dto = adGroupList.Map<List<ActiveDirectoryGroup>, List<ActiveDirectoryGroupDTO>>();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dto;
        }

        public List<NetworkDomainDTO> GetActiveDirectoryDomainList()
        {
            var dto = default(List<NetworkDomainDTO>);

            try
            {
                //Get domains list and casts directly to DTO
                dto = _adRepo.GetDomainList().Map<List<BaseNetworkDomain>, List<NetworkDomainDTO>>();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dto;
        }

        public ApplicationDTO GetApplication(ApplicationFilterDTO filter)
        {
            var dto = default(ApplicationDTO);

            try
            {
                if (string.IsNullOrEmpty(filter.ApplicationCode))
                    throw new ServiceException(CommonExceptionType.ParameterException, "ApplicationCode");

                //Select application and its profiles
                var ssoApp = GetSsoApplication(filter);

                //Initialize the DTO to return
                dto = ssoApp.Map<SsoApplication, ApplicationDTO>();

                //Set profiles, services and groups names
                foreach (var pro in ssoApp.Profiles)
                {
                    //Variables to use in loop
                    var profileId = pro.Id;
                    var dtoProfile = dto.Profiles.FirstOrDefault(p => p.Id == profileId);
                    dtoProfile.SourceDatabase = CommonDatabase.P335.ToString();

                    if (dtoProfile != null)
                    {
                        //Copy AD group list to the specified profile inside Dto
                        dtoProfile.ActiveDirectoryGroups = pro.ProfilesAndActiveDirectories
                            .Where(pad => pad.ProfileId == profileId)
                            .Select(pad => pad.AdGroup)
                            .ToList()
                            .Map<List<ActiveDirectoryGroup>, List<ActiveDirectoryGroupDTO>>();

                        //Copy worker list to the specified profile inside Dto
                        dtoProfile.Users = pro.ProfilesAndWorkers
                            .Select(paw => new SsoUserDTO
                            {
                                Id = paw.WorkerOrEmployeeId,
                                NameOrDescription = paw.Login,
                            })
                            .ToList();

                        //Inverts the hierarchy between SSOGroup and SSOServices, putting SSOGroup as a container
                        dtoProfile.SsoGroups = pro.ProfilesAndServices
                            .Where(p => p.ProfileId == profileId)
                            .Select(p => new SsoGroupDTO
                            {
                                Id = p.Service.GroupId,
                                NameOrDescription = p.Service.SsoGroup.NameOrDescription,
                                Services = pro.ProfilesAndServices
                                    .Where(ps => ps.ProfileId == profileId && ps.Service.GroupId == p.Service.GroupId)
                                    .Select(se => se.Service)
                                    .ToList()
                                    .Map<List<SsoServices>, List<SsoServicesDTO>>(),
                            })
                            .Distinct(new EqualBy<SsoGroupDTO>((x, y) => x.Id == y.Id))
                            .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dto;
        }

        public List<ApplicationDTO> GetApplicationsList(ApplicationListFilterDTO filter)
        {
            var dtos = new List<ApplicationDTO>();

            try
            {
                //Select application and its profiles
                var ssoApps = _ssoApplicationRepo.SelectWhere(app => app.IsInactive.ToUpper() == "N").ToList();

                var validations = new List<ValidationResult>();
                ssoApps.AsParallel().ForAll(apps => { apps.Validate(); validations.AddRange(apps.Validation.Results); });

                if (validations.Any())
                    throw new ServiceException(validations);

                var appsIds = ssoApps.Select(sa => sa.Id.ToLower()).ToList();
                var translationParameters = new List<string> { CommonTranslationParameter.TRANS_TP_EXC_APPL.ToString() };
                var translations = LoadTranslations(appsIds, translationParameters, filter.LanguageCultureName);

                //Adds profiles to applications
                ssoApps.AsParallel().ForAll(app => app.AddTranslations(translations));

                //Initialize the DTO to return
                dtos = ssoApps.Map<List<SsoApplication>, List<ApplicationDTO>>();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return dtos;
        }

        public SsoAuthorizationDTO GetApplicationAuthorizations(ApplicationFilterDTO filter)
        {
            var authorization = new SsoAuthorizationDTO();

            try
            {
                if (string.IsNullOrEmpty(filter.ApplicationCode))
                    throw new ServiceException(CommonExceptionType.ParameterException, "ApplicationCode");

                //Get application and its profiles
                var ssoApp = GetSsoApplication(filter);

                if (ssoApp != default(SsoApplication))
                {
                    authorization.Claims = ssoApp.GetClaims();
                    authorization.IsValid = (authorization.Claims.Count > 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return authorization;
        }

        public SsoAuthorizationDTO ValidateAndGetUserAuthorizations(SsoAuthenticationDTO sso)
        {
            var authorization = new SsoAuthorizationDTO { IsValid = false };

            try
            {
                if (string.IsNullOrEmpty(sso.EncriptedAppCode) ||
                    string.IsNullOrEmpty(sso.EncriptedLogin))
                    throw new ServiceException(CommonExceptionType.ParameterException, "EncriptedAppCode and EncriptedLogin");

                var appCode = CryptographHelper.RijndaelDecrypt(sso.EncriptedAppCode, CommonConsts.CommonPassword);
                var login = CryptographHelper.RijndaelDecrypt(sso.EncriptedLogin, CommonConsts.CommonPassword);
                var userFilter = new UserFilterDTO { Login = login, LoadProfiles = true };

                //Get user data
                var worker = GetWorker(userFilter);

                //Validates user password if its a SSO user
                worker.ValidateUserCredential(sso.EncriptedPassword);

                //Get worker related apps filtered by AppCode
                worker.Applications = GetUserApplications(userFilter, new ApplicationFilterDTO
                {
                    ApplicationCode = appCode,
                    LoadTranslations = true,
                    LanguageCultureName = sso.LanguageCultureName
                });

                //Transforms user permissions to claims identity
                authorization.Claims = worker.GetClaims();
                authorization.IsValid = (!worker.Validation.HasErrors && authorization.Claims.Count > 0);
            }
            catch (ServiceException ex)
            {
                //Suppress validations exceptions and returns an empty authorization
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionAndThrow(ex);
            }

            return authorization;
        }

        private Worker GetWorker(UserFilterDTO filter)
        {
            var query = new List<Expression<Func<Worker, bool>>>();
            var worker = default(Worker);

            if (!string.IsNullOrEmpty(filter.Id))
                query.Add(w => w.Id.ToLower() == filter.Id.ToLower());

            if (!string.IsNullOrEmpty(filter.Login))
                query.Add(w => w.Login.ToLower() == filter.Login.ToLower());

            //Try to get workers data at ActiveDirectory
            if (!string.IsNullOrEmpty(filter.Login))
                worker = _adRepo.GetUser(filter.Login, true).Map<BaseActiveDirectoryUser, Worker>();

            //Validates basic infos and throws an error if it fails
            worker.Validate();
            worker.BranchLine = (worker.UserExtraInfo.Phone ?? worker.BranchLine);

            if (worker.Validation.HasErrors)
                throw new ServiceException(worker.Validation.Results);

            //Try to get Corporative Worker data
            try
            {
                var workerCorporate = default(Worker);

                //Initializes unit of work and connections for Corporate Database
                using (var uowCorporate = new UnitOfWork<CorporateContextFmw>())
                {
                    //Get aditional data from Corporate Database
                    using (var workerRepo = uowCorporate.GetRepository<Worker>())
                    {
                        workerCorporate = workerRepo.Get(query.BuildExpression());
                    }
                }

                //Maps corporate data null properties and other datas
                if (workerCorporate != null)
                {
                    //Maps the Corporate Id, if its null uses the same SSO Id
                    worker.Id = (workerCorporate.Id ?? worker.Id);
                    worker = worker.MapOnlyNulls<Worker>(workerCorporate);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex);
            }

            //Try to get SSO Worker data
            try
            {
                //Get worker at SSO database
                var workerSSO = _workerRepo.Get(query.BuildExpression());

                //Maps sso data null properties and other datas
                if (workerSSO != null)
                {
                    //Maps the SSO Id, if its null uses the same SSO Id
                    worker.Id = (workerSSO.Id ?? worker.Id);
                    worker = worker.MapOnlyNulls<Worker>(workerSSO);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex);
            }

            return worker;
        }

        private List<SsoApplication> GetUserApplications(UserFilterDTO userFilter, ApplicationFilterDTO appFilter)
        {
            var appList = default(List<SsoApplication>);
            var query = new List<Expression<Func<SsoProfileAndWorker, bool>>> { wp => wp.Login.ToLower() == userFilter.Login.Trim().ToLower() };

            //Get distinct App ids
            var appDistinctIds = _ssoProfileWorkerRepo
                .SelectWhere(query.BuildExpression())
                .Select(paw => paw.ApplicationId.ToLower())
                .Distinct()
                .ToList();

            //Get all user related Applications
            appList = _ssoApplicationRepo
                .SelectWhere(app => appDistinctIds.Contains(app.Id.ToLower()))
                .ToList();

            //If an application code provided use to filter Profile querys.
            if (!string.IsNullOrEmpty(appFilter.ApplicationCode))
            {
                //Get application by Id, if not found try get by Mnemonic
                var ssoApp = appList.FirstOrDefault(app => app.Id.ToLower() == appFilter.ApplicationCode.ToLower());

                if (ssoApp == null)
                    ssoApp = appList
                        .Where(app => app.Mnemonic != null)
                        .FirstOrDefault(app => app.Mnemonic.ToLower() == appFilter.ApplicationCode.ToLower());

                if (ssoApp == null)
                    throw new ServiceException(CommonExceptionType.ValidationException, "Application isn´t related to this user.");

                query.Add(wp => wp.ApplicationId.ToLower() == ssoApp.Id.ToLower());
            }

            var allProfiles = new List<SsoProfile>();

            //Load User Profiles
            if (userFilter.LoadProfiles)
            {
                //Get profiles by workers
                var profilesWorkers = _ssoProfileWorkerRepo
                    .SelectWhere(query.BuildExpression(),
                    "Profile",
                    "Profile.ProfilesAndActiveDirectories",
                    "Profile.ProfilesAndActiveDirectories.AdGroup",
                    "Profile.ProfilesAndServices",
                    "Profile.ProfilesAndServices.Service",
                    "Profile.ProfilesAndServices.Service.SsoGroup")
                    .ToList();

                //Adds workers profiles to list
                allProfiles.AddRange(profilesWorkers.Select(paw => paw.Profile));
            }

            //Load Public Services, Groups and Profiles
            if (userFilter.LoadPublicServices)
            {
                //Get public services and nested objects
                var publicProfileServices = _ssoProfileServiceRepo
                    .SelectWhere(ps => ps.Service.IsPublic == "S",
                    "Profile",
                    "Service",
                    "Service.SsoGroup")
                    .ToList();

                //Adds public service profiles to list
                allProfiles.AddRange(publicProfileServices.Select(pps => pps.Profile));
            }

            //If profiles are loaded, put then under each app
            if (allProfiles.Any())
            {
                //Load translations for profiles and nested objects
                var translations = default(List<ApplicationTranslation>);
                if (appFilter.LoadTranslations)
                {
                    var translationParameters = new List<string>
                    {
                        CommonTranslationParameter.TRANS_TP_EXC_APPL.ToString(),
                        CommonTranslationParameter.TRANS_TP_EXC_GRUPO.ToString(),
                        CommonTranslationParameter.TRANS_TP_EXC_SERVICO.ToString(),
                        CommonTranslationParameter.TRANS_TP_EXC_GRP_EXB.ToString()
                    };

                    translations = LoadTranslations(appDistinctIds, translationParameters, appFilter.LanguageCultureName);
                }

                //Adds profiles to applications
                appList.AsParallel().ForAll(app =>
                {
                    app.AddProfilesAndDependencies(allProfiles);

                    if (appFilter.LoadTranslations)
                        app.AddTranslations(translations);
                });
            }

            return appList;
        }

        private SsoApplication GetSsoApplication(ApplicationFilterDTO filter)
        {
            //Select application and its profiles
            SsoApplication ssoApp = _ssoApplicationRepo.Get(app =>
                app.Id.ToLower() == filter.ApplicationCode.ToLower() ||
                app.Mnemonic.ToLower() == filter.ApplicationCode.ToLower());

            ssoApp.Validate();
            if (ssoApp.Validation.HasErrors)
                throw new ServiceException(ssoApp.Validation.Results);

            var validations = new List<ValidationResult>();

            //Gets profile, services and workers
            ssoApp.Profiles = _ssoProfileRepo.SelectWhere(sp => sp.ApplicationId.ToLower() == ssoApp.Id.ToLower(),
                                                    "ProfilesAndActiveDirectories",
                                                    "ProfilesAndActiveDirectories.AdGroup",
                                                    "ProfilesAndServices",
                                                    "ProfilesAndServices.Service",
                                                    "ProfilesAndServices.Service.SsoGroup")
                                                    .ToList();

            //Validates profile basic properties and adds to validation list
            ssoApp.Profiles.AsParallel().ForAll(po => { po.Validate(); validations.AddRange(po.Validation.Results); });

            //Load profiles for all users and validate errors
            var profilesWorkersList = default(List<SsoProfileAndWorker>);
            if (filter.LoadUsersProfiles)
            {
                profilesWorkersList = _ssoProfileWorkerRepo.SelectWhere(spw => spw.ApplicationId.ToLower() == ssoApp.Id.ToLower()).ToList();
                profilesWorkersList.AsParallel().ForAll(po => { po.Validate(); validations.AddRange(po.Validation.Results); });
            }

            //Throw validation messages
            if (validations.Any())
                throw new ServiceException(validations);

            var adGroupTempCache = new Hashtable();

            //Set profiles dependencies on profile list
            foreach (var pl in ssoApp.Profiles)
            {
                pl.ProfilesAndActiveDirectories
                    .ForEach(pad =>
                    {
                        if (pad.AdGroup != null && !string.IsNullOrEmpty(pad.AdGroup.NameOrDescription))
                        {
                            var tempKey = pad.AdGroup.NameOrDescription.Clone();
                            pad.AdGroup = ((ActiveDirectoryGroup)adGroupTempCache[tempKey] ?? _adRepo.GetGroup(pad.AdGroup.NameOrDescription, filter.LoadActiveDirectoryExtraInfo).Map<BaseActiveDirectoryGroup, ActiveDirectoryGroup>());
                            adGroupTempCache[tempKey] = pad.AdGroup;
                        }
                        else
                            pad.AdGroup = new ActiveDirectoryGroup { Users = new List<BaseIdentification>() };
                    });

                pl.ProfilesAndServices
                    .AsParallel().ForAll(pas =>
                    {
                        if (pas.Service == null)
                            pas.Service = new SsoServices { SsoGroup = new SsoGroup() };
                        else if (pas.Service.SsoGroup == null)
                            pas.Service.SsoGroup = new SsoGroup();
                    });

                if (filter.LoadUsersProfiles)
                    pl.ProfilesAndWorkers = profilesWorkersList
                        .Where(pw => pw.ProfileId == pl.Id && pw.ApplicationId == pl.ApplicationId)
                        .ToList();
            }

            //Load translations for profiles and nested objects
            if (filter.LoadTranslations)
            {
                var appIds = new List<string> { ssoApp.Id.ToLower() };
                var translationParameters = new List<string>
                    {
                        CommonTranslationParameter.TRANS_TP_EXC_APPL.ToString(),
                        CommonTranslationParameter.TRANS_TP_EXC_GRUPO.ToString(),
                        CommonTranslationParameter.TRANS_TP_EXC_SERVICO.ToString(),
                        CommonTranslationParameter.TRANS_TP_EXC_GRP_EXB.ToString()
                    };

                var translations = LoadTranslations(appIds, translationParameters, filter.LanguageCultureName);

                ssoApp.AddTranslations(translations);
            }

            return ssoApp;
        }

        private List<ApplicationTranslation> LoadTranslations(List<string> appIds, List<string> translationParameterNames, string languageCultureName)
        {
            languageCultureName = (languageCultureName ?? LanguageCultureName).ToLower();

            //Select translation ids and casts to integers
            var translationTypes = _appParameterRepo
                .SelectWhere(at => translationParameterNames.Contains(at.ParameterName.ToUpper()))
                .ToList();

            var translationIds = translationTypes.Select(tt => tt.Content.ToInt());

            //Gets all translations for an Application and Language Culture
            var translations = _appTranslationRepo.SelectWhere(at =>
                at.LanguageCultureName.ToLower() == languageCultureName &&
                appIds.Contains(at.ApplicationId.ToLower()) &&
                translationIds.Contains(at.TranslationTypeId))
                .ToList();

            if (translations != null && translations.Any())
            {
                //Merges translations with its base definitions
                translations.AsParallel().ForAll(t =>
                {
                    t.TranslationType = translationTypes
                        .Where(tt => tt.Content.ToInt() == t.TranslationTypeId)
                        .FirstOrDefault();
                });
            }

            return translations;
        }

        public void Dispose()
        {
            _adRepo = null;
            _workerRepo = null;
            _ssoApplicationRepo = null;
            _ssoProfileRepo = null;
            _ssoProfileWorkerRepo = null;
            _appParameterRepo = null;
            _appTranslationRepo = null;

            Uow.Dispose();
            Uow = null;
        }
    }
}