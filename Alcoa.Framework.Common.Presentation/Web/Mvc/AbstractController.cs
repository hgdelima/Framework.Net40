using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Exceptions;
using Alcoa.Framework.Common.Presentation.Enumerator;
using Alcoa.Framework.Common.Presentation.GarService;
using Alcoa.Framework.Common.Presentation.Properties;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Caching;
using System.Web.Mvc;

namespace Alcoa.Framework.Common.Presentation.Web.Mvc
{
    /// <summary>
    /// Abstract controller to be used by MVC controllers that do not want load User and GAR data
    /// </summary>
    public abstract class AbstractController : Controller
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
                    throw new PresentationException(CommonExceptionType.AppInitializationException, "Error when getting current User data. " + ex.GetAllExceptionMessages());
                }

                return null;
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

        private HeaderModel _header;
        protected HeaderModel Header
        {
            get
            {
                if (_header == default(HeaderModel) ||
                    (_header != default(HeaderModel) &&
                    _header.CurrentSelectedLanguage != Thread.CurrentThread.CurrentCulture.Name))
                {
                    //Load localization Labels and Messages
                    _header = new HeaderModel();
                    _header.AppCode = AppCode;
                    _header.LabelAppTitle = Resources.LabelAppTitle;
                    _header.LabelMessageTitle = Resources.LabelMessageTitle;
                    _header.LabelMenuHelp = Resources.LabelMenuHelp;
                    _header.LabelMenuAbout = Resources.LabelMenuAbout;
                    _header.LabelMenuOpenTicket = Resources.LabelMenuOpenTicket;
                    _header.LabelMenuMyApps = Resources.LabelMenuMyApps;
                    _header.LabelUserTitle = Resources.LabelUserTitle;
                    _header.LabelUserName = Resources.LabelUserName;
                    _header.LabelExtensionLine = Resources.LabelExtensionLine;
                    _header.LabelLanguage = Resources.LabelLanguage;
                    _header.LabelLogOut = Resources.LabelLogout;
                    _header.ServiceDeskLine = Resources.ServiceDeskDirectLine;
                    _header.ServiceDeskExtension = Resources.ServiceDeskExtension;
                    _header.LabelAppCode = Resources.LabelAppCode;
                    _header.LabelAppVersion = Resources.LabelAppVersion;
                    _header.LabelServiceDeskContact = Resources.LabelServiceDeskContact;
                    _header.LabelDirectLine = Resources.LabelDirectLine;
                    _header.LabelGarTitle = Resources.LabelGarTitle;
                    _header.LabelGarLeadersTitle = Resources.LabelGarLeadersTitle;
                    _header.LabelGarMainLeader = Resources.LabelGarMainLeader;
                    _header.LabelGarBackupLeader = Resources.LabelGarBackupLeader;
                    _header.LabelGarBPOsTitle = Resources.LabelGarBPOsTitle;
                    _header.LabelGarMainBPO = Resources.LabelGarMainBPO;
                    _header.LabelGarBackupBPO = Resources.LabelGarBackupBPO;
                    _header.CurrentSelectedLanguage = Thread.CurrentThread.CurrentCulture.Name;

                    //Pre loaded data
                    _header.AppVersion = "1.0";
                }

                return _header;
            }
        }

        /// <summary>
        /// Abstract controller to be used by controllers that don´t need Users data
        /// </summary>
        public AbstractController()
        {
            ViewBag.AppCode = Header.AppCode;
        }

        [AllowAnonymous]
        public virtual ActionResult GetHeader()
        {
            var environment = Request.Url.Host;
            var environmentCode = string.Empty;
            Header.CssAlcoaHeader = "alcoa-header-prod";

            //Loads Environment data
            try
            {
                //Check the current environment to apply Header CSS
                if (CommonEnvironmentConsts.DevEnvironmentUrls.Any(du => environment.StartsWith(du, StringComparison.OrdinalIgnoreCase)))
                {
                    Header.CssAlcoaHeader = "alcoa-header-dev";
                    environmentCode = " (DEV)";
                }
                else if (CommonEnvironmentConsts.QaEnvironmentUrls.Any(qu => environment.StartsWith(qu, StringComparison.OrdinalIgnoreCase)))
                {
                    Header.CssAlcoaHeader = "alcoa-header-qa";
                    environmentCode = " (QA)";
                }
            }
            catch (Exception ex)
            {
            }

            //Loads GAR data
            try
            {
                if (GarModel.AppGar != null)
                {
                    var garInstance = GarModel.AppGarInstances.FirstOrDefault();
                    var garUsage = GarModel.AppGarUsages.FirstOrDefault();

                    if (!string.IsNullOrEmpty(GarModel.AppGarUsageId))
                        garUsage = garInstance.GarUsages.FirstOrDefault(gu => gu.id == GarModel.AppGarUsageId.ToInt());

                    Header.AppVersion = "1.0"; //Not provided by GAR service 
                    Header.AppTitle = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(GarModel.AppGar.name) + environmentCode;

                    if (garInstance.leader1 != null)
                    {
                        Header.GarMainLeader = garInstance.leader1.DisplayName;
                        Header.GarMainLeaderEmail = garInstance.leader1.Email;
                    }

                    if (garInstance.leader2 != null)
                    {
                        Header.GarBackupLeader = garInstance.leader2.DisplayName;
                        Header.GarBackupLeaderEmail = garInstance.leader2.Email;
                    }

                    if (garUsage != null)
                    {
                        if (!string.IsNullOrEmpty(GarModel.AppGarUsageId))
                            Header.AppTitle = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(garUsage.name) + environmentCode;

                        if (garUsage.bpo1 != null)
                        {
                            Header.GarMainBPO = garUsage.bpo1.DisplayName;
                            Header.GarMainBPOEmail = garUsage.bpo1.Email;
                        }

                        if (garUsage.bpo2 != null)
                        {
                            Header.GarBackupBPO = garUsage.bpo2.DisplayName;
                            Header.GarBackupBPOEmail = garUsage.bpo2.Email;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return PartialView("Header", Header);
        }

        [AllowAnonymous]
        public virtual ActionResult GetFooter()
        {
            try
            {
                Header.LabelFooter = string.Format(Resources.LabelFooter, DateTime.Now.Year, Header.AppVersion);
            }
            catch (Exception ex)
            {
            }

            return PartialView("Footer", Header);
        }

        /// <summary>
        /// Create the left menu to current Application
        /// </summary>
        [AllowAnonymous]
        public virtual JsonResult GetApplicationMenu()
        {
            var appMenu = new List<dynamic>();
            var cacheKey = string.Empty;

            try
            {
                if (UserClaims != null)
                {
                    //Creates a cacheKey using user login
                    cacheKey = "appMenu" + UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.WindowsAccountName).Value;
                    appMenu = (List<dynamic>)HttpContext.Cache.Get(cacheKey);
                }

                if (appMenu == null || appMenu.Count <= 0)
                {
                    var menu = AppMenu.GetApplicationMenu();
                    appMenu = AppMenu.CastToDynamicMenuList(menu);

                    if (!string.IsNullOrEmpty(cacheKey))
                        HttpContext.Cache.Add(cacheKey, appMenu, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }
            }
            catch (Exception ex)
            {
            }

            return Json(appMenu, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create the right menu to all user authorized applications
        /// </summary>
        [AllowAnonymous]
        public virtual JsonResult GetMyAppsMenu()
        {
            var myAppsMenu = new List<dynamic>();
            var cacheKey = string.Empty;

            try
            {
                var menuList = default(List<MenuModel>);

                if (UserClaims != null)
                {
                    //Creates a cacheKey using user login
                    cacheKey = "myAppsMenu" + UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.WindowsAccountName).Value;
                    menuList = (List<MenuModel>)HttpContext.Cache.Get(cacheKey);
                }

                if (menuList == null || menuList.Count <= 0)
                {
                    menuList = AppMenu.GetMyAppsMenu(AppCode, Header.LabelMenuMyApps);

                    if (!string.IsNullOrEmpty(cacheKey))
                        HttpContext.Cache.Add(cacheKey, menuList, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }

                //Changes menu label for current Culture
                menuList.FirstOrDefault().Text = Header.LabelMenuMyApps;
                myAppsMenu = AppMenu.CastToDynamicMenuList(menuList);
            }
            catch (Exception ex)
            {
            }

            return Json(myAppsMenu, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create the right menu with about infos
        /// </summary>
        [AllowAnonymous]
        public virtual JsonResult GetAboutMenu()
        {
            var aboutMenu = new List<dynamic>();

            try
            {
                var menuList = AppMenu.GetAboutMenu(Header.LabelMenuHelp, Header.LabelMenuOpenTicket, Header.LabelMenuAbout);
                aboutMenu = AppMenu.CastToDynamicMenuList(menuList);
            }
            catch (Exception ex)
            {
            }

            return Json(aboutMenu, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Change user login in current application
        /// </summary>
        [AllowAnonymous]
        public virtual ActionResult ChangeUser()
        {
            var federationModule = FederatedAuthentication.WSFederationAuthenticationModule;

            if (federationModule != null)
            {
                if (!string.IsNullOrEmpty(federationModule.Reply) &&
                    !string.IsNullOrEmpty(federationModule.Issuer))
                {
                    var replyUrl = new Uri(FederatedAuthentication.WSFederationAuthenticationModule.Reply);
                    var issuer = new Uri(FederatedAuthentication.WSFederationAuthenticationModule.Issuer);
                    WSFederationAuthenticationModule.FederatedSignOut(issuer, replyUrl);
                }
            }

            return View();
        }
    }
}