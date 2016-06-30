using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Helpers;
using Alcoa.Framework.Common.Presentation.Enumerator;
using Microsoft.IdentityModel.Web;
using System;
using System.Globalization;
using System.Security;
using System.Threading;
using System.Web;
using System.Web.Routing;

namespace Alcoa.Framework.Common.Presentation.Web.Mvc
{
    /// <summary>
    /// Abstract Global Asax to be used by MVC applications
    /// </summary>
    public abstract class AbstractGlobalAsax : HttpApplication
    {
        private const string languageParam = "language";

        /// <summary>
        /// Execute when application receives a request
        /// </summary>
        protected virtual void Application_BeginRequest(Object sender, EventArgs e)
        {
            var selectedLanguage = Request.QueryString.Get(languageParam);
            var cookieLanguage = Request.Cookies.Get(languageParam);

            //Check if it a language was selected or cookie was value
            if (selectedLanguage == null && cookieLanguage == null)
            {
                selectedLanguage = Thread.CurrentThread.CurrentCulture.IetfLanguageTag;

                var cookie = new HttpCookie(languageParam, selectedLanguage) { Expires = DateTime.Now.AddDays(1) };
                Response.Cookies.Add(cookie);
            }
            else if (selectedLanguage != null && cookieLanguage != null && selectedLanguage != cookieLanguage.Value)
            {
                var cookie = new HttpCookie(languageParam, selectedLanguage) { Expires = DateTime.Now.AddDays(1) };
                Response.Cookies.Add(cookie);
            }
            else
                selectedLanguage = cookieLanguage.Value;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(selectedLanguage);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);

            HttpContext.Current.Items.Add("HttpResponse", Response);
        }

        /// <summary>
        /// Execute when a error is throw within application
        /// </summary>
        protected virtual void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();

            if (ex == null)
                return;

            Server.ClearError();
            Response.Clear();
            Response.TrySkipIisCustomErrors = true;

            var routeData = new RouteData();
            routeData.Values[CommonRouteConsts.Controller] = "Error";
            routeData.Values[CommonRouteConsts.Action] = "Index";
            routeData.Values[CommonRouteConsts.Message] = ex.Message;
            routeData.Values[CommonRouteConsts.Details] = ex.GetAllExceptionMessages();

            if (ex is ArgumentException)
            {
                routeData.Values[CommonRouteConsts.Message] = "Invalid or null request.";
            }
            else if (ex is SecurityException)
            {
                routeData.Values[CommonRouteConsts.Message] = "Access Denied.";
            }
            else if (ex is HttpException)
            {
                switch (((HttpException)ex).GetHttpCode())
                {
                    case 404:
                        routeData.Values[CommonRouteConsts.Message] = "Page not found.";
                        break;

                    case 500:
                        routeData.Values[CommonRouteConsts.Message] = "Internal server error.";
                        break;
                }
            }

            Response.RedirectToRoute(routeData.Values);
        }

        /// <summary>
        /// Execute when an user enters the application
        /// </summary>
        protected virtual void Session_Start(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Execute when an user leaves the application
        /// </summary>
        protected virtual void Session_End(object sender, EventArgs e)
        {
            //var replyUrl = new Uri(FederatedAuthentication.WSFederationAuthenticationModule.Reply);
            //var issuer = new Uri(FederatedAuthentication.WSFederationAuthenticationModule.Issuer);

            //WSFederationAuthenticationModule.FederatedSignOut(issuer, replyUrl);
        }
    }
}