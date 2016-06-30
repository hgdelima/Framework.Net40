using Microsoft.IdentityModel.Claims;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web;

namespace Alcoa.Framework.Common.Presentation.SSO
{
    /// <summary>
    /// Class where claims based applications performs authorization validations
    /// </summary>
    /// <remarks>Can't be debugged</remarks>
    [Browsable(false), DebuggerStepThrough]
    public class SsoModule : IHttpModule
    {
        /// <summary>
        /// Module Initialization and event bounds
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public void Init(HttpApplication application)
        {
            application.AuthorizeRequest += this.Application_AuthorizeRequest;
        }

        [Browsable(false), DebuggerHidden]
        private void Application_AuthorizeRequest(Object source, EventArgs e)
        {
            //Create HttpContext object to access request and response properties.
            var context = ((HttpApplication)source).Context;
            var identity = Thread.CurrentPrincipal.Identity; 

            if (identity.IsAuthenticated &&
                identity.AuthenticationType == AuthenticationTypes.Federation)
                ClaimsPrincipalPermission.CheckAccess(context.Request.Url.OriginalString, context.Request.HttpMethod);
        }

        /// <summary>
        /// Dispose to release resources
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public void Dispose()
        {
        }
    }
}