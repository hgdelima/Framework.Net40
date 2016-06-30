using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Presentation.Enumerator;
using Microsoft.IdentityModel.Web;
using Microsoft.IdentityModel.Web.Controls;
using System;
using System.Web;

namespace Alcoa.Framework.Common.Presentation.SSO
{
    /// <summary>
    /// This corrects WIF error ID3206 "A SignInResponse message may only redirect within the current web application: when ends in /"
    /// </summary>
    public class SsoWSFederationAuthenticationModule : WSFederationAuthenticationModule
    {
        protected override void OnSignInError(ErrorEventArgs args)
        {
            base.OnSignInError(args);
        }

        /// <summary>
        /// Compare if Request Url + "/" is equal to the Realm, so only root access is corrected
        /// https://localhost/AppName plus "/" is equal to https://localhost/AppName/
        /// </summary>
        public override void RedirectToIdentityProvider(string uniqueId, string returnUrl, bool persist)
        {
            var appCode = string.IsNullOrEmpty(ConfigHelper.GetAppSetting(CommonAppSettingKeyName.SsoApplicationCode)) ?
                ConfigHelper.GetAppSetting(CommonAppSettingKeyName.ApplicationCode) :
                ConfigHelper.GetAppSetting(CommonAppSettingKeyName.SsoApplicationCode);

            //Create an sign in parameter wappcode=XYZ
            var signInParameter = CommonWsFederationConsts.ApplicationCode + "=" + appCode;

            if (!SignInQueryString.Contains(signInParameter))
                SignInQueryString += signInParameter;

            //First Check if the request url doesn't end with a "/"
            if (!returnUrl.EndsWith("/"))
            {
                var absoluteUri = HttpContext.Current.Request.Url.AbsoluteUri + "/";

                //Add the trailing slash
                if (string.Compare(absoluteUri, base.Realm, StringComparison.InvariantCultureIgnoreCase) == default(int))
                    returnUrl += "/";
            }

            base.RedirectToIdentityProvider(uniqueId, returnUrl, persist);
        }
    }
}
