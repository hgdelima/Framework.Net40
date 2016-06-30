using Alcoa.Framework.Common.Presentation.Enumerator;
using Alcoa.Framework.Common.Presentation.Web.Mvc;
using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Alcoa.Framework.Common.Presentation.SSO
{
    /// <summary>
    /// Class where claims based applications performs authorization validations
    /// </summary>
    /// <remarks>Can't be debugged</remarks>
    [Browsable(false), DebuggerStepThrough] 
    public class SsoAuthorizationManager : ClaimsAuthorizationManager
    {
        private static List<string> PreAuthorizedControllers = new List<string> { "/?", "/Welcome", "/Error" };

        /// <summary>
        /// Custom SSO authorization management
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public SsoAuthorizationManager()
        {            
        }

        /// <summary> 
        /// Checks if the principal specified in the authorization context is authorized to perform action
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public override bool CheckAccess(AuthorizationContext authContext)
        {
            //Initializes access has true
            var access = true;
            var resourceUri = new Uri(authContext.Resource.First().Value);
            var controller = GetControllerPath(resourceUri);

            try
            {
                //Validates root path "/"
                if (resourceUri.AbsolutePath != "/")
                {
                    //Validates pre authorized urls in controller name
                    if (!string.IsNullOrEmpty(controller) &&
                        PreAuthorizedControllers.All(pa => !controller.StartsWith(pa, StringComparison.OrdinalIgnoreCase)))
                    {
                        var userClaims = ((IClaimsIdentity)authContext.Principal.Identity).Claims;

                        //Filters service names inside each claim
                        var services = userClaims
                            .Select(cc => cc.Value)
                            .Where(s => s != "/")
                            .ToList();

                        //Matches controller values
                        if (services.Any())
                            access = services.Any(se => se.Contains(controller) || controller.Contains(se));
                        else
                            access = false;
                    }
                }
            }
            catch (Exception ex)
            {
                access = false;
            }

            return access;
        }

        /// <summary> 
        /// Get controller name
        /// </summary>
        [Browsable(false), DebuggerHidden]
        private string GetControllerPath(Uri resourceUri)
        {
            var controller = string.Empty;

            try
            {
                //Parse to a route table to get Controller and Actions
                var route = UrlHelper.ConvertUrlToRoute(resourceUri);
                controller = (string)route.Values[CommonRouteConsts.Controller];

                //Adds a root path before controller name
                controller = "/" + controller;
            }
            catch (Exception)
            {
                //If can not create a route table and find a controller/page name
            }

            return controller;
        }
    }
}