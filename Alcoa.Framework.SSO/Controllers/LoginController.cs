using Alcoa.Framework.Application.Service;
using Alcoa.Framework.Common;
using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Presentation.Enumerator;
using Alcoa.Framework.SSO.Properties;
using Alcoa.Framework.Contract.DTOs;
using Alcoa.Framework.SSO.Models;
using Microsoft.IdentityModel.Claims;
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Resources;

namespace Alcoa.Framework.SSO.Controllers
{
    public class LoginController : Controller
    {
        private readonly SsoService ssoService = new SsoService();
        private SsoSigninSignoutDTO _ssoSigninSignout = default(SsoSigninSignoutDTO);

        public ActionResult Index()
        {
            ViewBag.RequestUrl = Request.Url;

            try
            {
                if (ValidateUserDomainAuthentication() && 
                    ValidateAndCreateSSO(Request.Url) &&
                    ValidateUserCredentialAndGetClaims(new LoginModel()))
                    return ExecuteAction();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Login", "Index"));
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel, string requestUrl)
        {
            ViewBag.RequestUrl = requestUrl;

            try
            {
                if (ModelState.IsValid)
                {
                    if (ValidateAndCreateSSO(new Uri(requestUrl)) &&
                        ValidateUserCredentialAndGetClaims(loginModel))
                        return ExecuteAction();

                    ModelState.AddModelError(string.Empty, CommonExceptionType.InvalidUsernameAndPassword.GetStringMessage());
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Login", "Index"));
            }

            return View("Index", loginModel);
        }

        /// <summary>
        /// Executes federation SignIn or SignOut
        /// </summary>
        private ActionResult ExecuteAction()
        {
            if (_ssoSigninSignout.RequestAction == SsoRequestParameter.WsSignIn.GetDescription())
            {
                var formData = ssoService.SignIn(_ssoSigninSignout);
                return new ContentResult { Content = formData, ContentType = "text/html" };
            }

            if (_ssoSigninSignout.RequestAction == SsoRequestParameter.WsSignOut.GetDescription())
                ssoService.SignOut(_ssoSigninSignout, (HttpResponse)HttpContext.Items["HttpResponse"]);

            return View();
        }

        /// <summary>
        /// Validates request parameters and web.config configurations
        /// </summary>
        private bool ValidateAndCreateSSO(Uri requestUrl)
        {
            var isValid = default(bool);
            var action = HttpUtility.ParseQueryString(requestUrl.Query).Get(SsoRequestParameter.WsAction.GetDescription());

            if (!string.IsNullOrEmpty(action))
            {
                if (string.IsNullOrWhiteSpace(ConfigHelper.GetAppSetting("IssuerName")))
                    throw new ConfigurationErrorsException(Resources.IssuerNameKeyNotFound);

                if (string.IsNullOrWhiteSpace(ConfigHelper.GetAppSetting("CertificateAppFolderAndName")) &&
                    string.IsNullOrWhiteSpace(ConfigHelper.GetAppSetting("CertificateSubjectName")))
                    throw new ConfigurationErrorsException(Resources.CertificateKeyNotFound);

                var certificateFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigHelper.GetAppSetting("CertificateAppFolderAndName"));

                var certificate = CertificateHelper.GetCertificateAtPath(certificateFullPath) ??
                    CertificateHelper.GetCertificateAtIIS(ConfigHelper.GetAppSetting("CertificateSubjectName"), StoreName.My);

                if (certificate == null)
                    throw new FileLoadException(Resources.CertificateLoadError);

                _ssoSigninSignout = new SsoSigninSignoutDTO
                {
                    RequestAction = action,
                    RequestUrl = requestUrl,
                    ReplyUrl = requestUrl,
                    ClaimsUser = new ClaimsPrincipal(new ClaimsIdentityCollection{ new ClaimsIdentity(HttpContext.User.Identity) }),
                    IssuerName = ConfigHelper.GetAppSetting("IssuerName"),
                    Certificate = certificate
                };

                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// Validates if user its already authenticated in domain
        /// </summary>
        private bool ValidateUserDomainAuthentication()
        {
            var currentIdentity = WindowsIdentity.GetCurrent();
            var isValid = currentIdentity != null && 
                          currentIdentity.IsAuthenticated &&
                          (currentIdentity.AuthenticationType == AuthenticationTypes.Kerberos || 
                           currentIdentity.AuthenticationType == AuthenticationTypes.Windows);

            return isValid;
        }

        /// <summary>
        /// Validates user using Windows or Forms authentication
        /// </summary>
        private bool ValidateUserCredentialAndGetClaims(LoginModel login)
        {
            //If its an signout action don´t get user data
            if (_ssoSigninSignout.RequestAction == SsoRequestParameter.WsSignOut.GetDescription())
                return true;

            //Encripts user data and password for forms authentication
            if (!string.IsNullOrEmpty(login.Username) &&
                !string.IsNullOrEmpty(login.Password))
            {
                login.Username = CryptographHelper.RijndaelEncrypt(login.Username, CommonFrameworkResource.CommonFrameworkPassword.GetDescription());
                login.Password = CryptographHelper.RijndaelEncrypt(login.Password, CommonFrameworkResource.CommonFrameworkPassword.GetDescription());
            }

            var sso = new SsoAuthenticationDTO
            {
                EncriptedLogin = login.Username,
                EncriptedPassword = login.Password,
                LanguageCultureName = Thread.CurrentThread.CurrentCulture.Name.ToUpper(),
            };

            var userIdentity = ssoService.ValidateUserAndGetClaims(sso);
            var userIsValid = (userIdentity != default(ClaimsIdentity) && userIdentity.IsAuthenticated);

            //Adds returned Claims Principal to SSO object
            if (userIsValid)
                _ssoSigninSignout.ClaimsUser = new ClaimsPrincipal(new ClaimsIdentityCollection {userIdentity});

            return userIsValid;
        }
    }
}