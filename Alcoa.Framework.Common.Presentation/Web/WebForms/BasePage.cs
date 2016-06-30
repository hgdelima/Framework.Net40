using Alcoa.Entity.Entity;
using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Exceptions;
using Alcoa.Framework.Common.Presentation.Enumerator;
using Alcoa.Framework.Common.Presentation.Properties;
using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI;

namespace Alcoa.Framework.Common.Presentation.Web.WebForms
{
    public class BasePage : Page
    {
        private static string _appCode;
        protected static string AppCode
        {
            get { return (_appCode ?? (_appCode = ConfigHelper.GetAppSetting(CommonAppSettingKeyName.ApplicationCode))); }
        }

        protected string AppPath
        {
            get
            {
                return (Request.ApplicationPath == "/") ?
                    string.Empty :
                    Request.ApplicationPath + "/";
            }
        }

        private BaseEmployee _appUser;
        protected BaseEmployee AppUser
        {
            get
            {
                try
                {
                    if (_appUser == null)
                    {
                        if (UserClaims == null)
                        {
                            _appUser = new BaseEmployee
                            {
                                Login = User.Identity.Name,
                                NameOrDescription = User.Identity.Name,
                            };
                        }
                        else
                        {
                            var emptyClaim = new Claim(string.Empty, string.Empty);

                            _appUser = new BaseEmployee
                            {
                                Id = UserClaims.FirstOrDefault(uc => uc.ClaimType == SsoClaimTypes.SsoId).Value,
                                Domain = UserClaims.FirstOrDefault(uc => uc.ClaimType == SsoClaimTypes.SsoDomain).Value,
                                NameOrDescription = UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Name).Value,
                                Email = UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Email).Value,
                                Login = UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.WindowsAccountName).Value,
                                Sid = UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Sid).Value,
                                Gender = UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Gender).Value,
                                BranchLine = UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.OtherPhone).Value,
                                BirthDate = (UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.DateOfBirth) ?? emptyClaim).Value.ToDateTimeNullable(),
                                LoginExpirationDate = (UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Expiration) ?? emptyClaim).Value.ToDateTimeNullable(),
                                UserExtraInfo = new BaseUserExtraInfo
                                {
                                    FirstName = (UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.GivenName) ?? emptyClaim).Value,
                                    LastName = (UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Surname) ?? emptyClaim).Value,
                                    Office = (UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Locality) ?? emptyClaim).Value,
                                },
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new PresentationException(CommonExceptionType.AppInitializationException, "Error when getting current User data. " + ex.GetAllExceptionMessages());
                }

                return _appUser;
            }
        }

        private MenuModel _appMenu;
        protected MenuModel AppMenu
        {
            get
            {
                if (_appMenu == null)
                {
                    //Initializes menu model with claims or nothing
                    if (UserClaims != null && UserClaims.Count > 0)
                        _appMenu = new MenuModel(UserClaims, AppPath);
                    else
                        _appMenu = new MenuModel();
                }

                return _appMenu;
            }
        }

        protected ClaimCollection UserClaims
        {
            get
            {
                try
                {
                    if (User != null &&
                        User.Identity != null &&
                        User.Identity is IClaimsIdentity)
                        return ((IClaimsIdentity)User.Identity).Claims;
                }
                catch (Exception ex)
                {
                    throw new PresentationException(CommonExceptionType.AppInitializationException, ex.GetAllExceptionMessages());
                }

                return null;
            }
        }

        private HeaderModel _headerModel;
        protected HeaderModel HeaderModel
        {
            get
            {
                if (_headerModel == default(HeaderModel) ||
                    (_headerModel != default(HeaderModel) &&
                    _headerModel.CurrentSelectedLanguage != Thread.CurrentThread.CurrentCulture.Name))
                {
                    //Load localization Labels and Messages
                    _headerModel = new HeaderModel();
                    _headerModel.AppCode = AppCode;
                    _headerModel.LabelAppTitle = Resources.LabelAppTitle;
                    _headerModel.LabelMessageTitle = Resources.LabelMessageTitle;
                    _headerModel.LabelMenuHelp = Resources.LabelMenuHelp;
                    _headerModel.LabelMenuAbout = Resources.LabelMenuAbout;
                    _headerModel.LabelMenuOpenTicket = Resources.LabelMenuOpenTicket;
                    _headerModel.LabelMenuMyApps = Resources.LabelMenuMyApps;
                    _headerModel.LabelUserTitle = Resources.LabelUserTitle;
                    _headerModel.LabelUserName = Resources.LabelUserName;
                    _headerModel.LabelExtensionLine = Resources.LabelExtensionLine;
                    _headerModel.LabelLanguage = Resources.LabelLanguage;
                    _headerModel.LabelLogOut = Resources.LabelLogout;
                    _headerModel.ServiceDeskLine = Resources.ServiceDeskDirectLine;
                    _headerModel.ServiceDeskExtension = Resources.ServiceDeskExtension;
                    _headerModel.LabelAppCode = Resources.LabelAppCode;
                    _headerModel.LabelAppVersion = Resources.LabelAppVersion;
                    _headerModel.LabelServiceDeskContact = Resources.LabelServiceDeskContact;
                    _headerModel.LabelDirectLine = Resources.LabelDirectLine;
                    _headerModel.LabelGarTitle = Resources.LabelGarTitle;
                    _headerModel.LabelGarLeadersTitle = Resources.LabelGarLeadersTitle;
                    _headerModel.LabelGarMainLeader = Resources.LabelGarMainLeader;
                    _headerModel.LabelGarBackupLeader = Resources.LabelGarBackupLeader;
                    _headerModel.LabelGarBPOsTitle = Resources.LabelGarBPOsTitle;
                    _headerModel.LabelGarMainBPO = Resources.LabelGarMainBPO;
                    _headerModel.LabelGarBackupBPO = Resources.LabelGarBackupBPO;
                    _headerModel.CurrentSelectedLanguage = Thread.CurrentThread.CurrentCulture.Name;

                    //Pre loaded data
                    _headerModel.AppVersion = "1.0";
                }

                return _headerModel;
            }
        }

        public virtual List<MenuModel> GetApplicationMenu()
        {
            var appMenu = new List<MenuModel>();

            try
            {
                appMenu = AppMenu.GetApplicationMenu();
            }
            catch (Exception ex)
            {
            }

            return appMenu;
        }

        public virtual List<MenuModel> GetMyAppsMenu()
        {
            var myAppsMenu = new List<MenuModel>();

            try
            {
                myAppsMenu = AppMenu.GetMyAppsMenu(AppCode, HeaderModel.LabelMenuMyApps);
            }
            catch (Exception ex)
            {
            }

            return myAppsMenu;
        }

        public virtual List<MenuModel> GetAboutMenu()
        {
            var aboutMenu = new List<MenuModel>();

            try
            {
                aboutMenu = AppMenu.GetAboutMenu(HeaderModel.LabelMenuHelp, HeaderModel.LabelMenuOpenTicket, HeaderModel.LabelMenuAbout);
            }
            catch (Exception ex)
            {
            }

            return aboutMenu;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var environment = Request.Url.Host;
            var environmentCode = string.Empty;
            HeaderModel.CssAlcoaHeader = "alcoa-header-prod";

            //Check the current environment to apply Header CSS
            if (CommonEnvironmentConsts.DevEnvironmentUrls.Any(du => environment.StartsWith(du, StringComparison.OrdinalIgnoreCase)))
            {
                HeaderModel.CssAlcoaHeader = "alcoa-header-dev";
                environmentCode = " (DEV)";
            }
            else if (CommonEnvironmentConsts.QaEnvironmentUrls.Any(qu => environment.StartsWith(qu, StringComparison.OrdinalIgnoreCase)))
            {
                HeaderModel.CssAlcoaHeader = "alcoa-header-qa";
                environmentCode = " (QA)";
            }

            if (AppUser != null)
            {
                HeaderModel.UserName = AppUser.NameOrDescription;
                HeaderModel.UserGender = AppUser.Gender;
                HeaderModel.UserEmail = AppUser.Email;
                HeaderModel.UserExtensionLine = AppUser.BranchLine;
            }
        }
    }
}
