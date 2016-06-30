using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Alcoa.Framework.Domain.Entity
{
    public class SsoProfileAndService : Base, IBaseDomain
    {
        public SsoProfileAndService()
        {
            Validation = new BaseValidation();
        }

        public string ApplicationId { get; set; }

        public string ProfileId { get; set; }

        public SsoProfile Profile { get; set; }

        public string ServiceId { get; set; }

        public SsoServices Service { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ApplicationId) &&
                string.IsNullOrEmpty(ProfileId) &&
                string.IsNullOrEmpty(ServiceId))
                Validation.Results.Add(new ValidationResult("Error: Invalid SsoProfileAndService object or was not found."));
        }

        #endregion

        /// <summary>
        /// Get service and nested objects as Claims list
        /// </summary>
        public List<BaseIdentification> GetClaims()
        {
            //Initialize claims
            var claims = new List<BaseIdentification>();

            if (Service.SsoGroup != null && !string.IsNullOrEmpty(Service.SsoGroup.NameOrDescription))
            {
                //Creates claim values using Ids and adds to claims list
                var ssoGroupClaim = ApplicationId + "|" + ProfileId + "|" + Service.SsoGroup.NameOrDescription.Trim();
                claims.Add(new BaseIdentification(SsoClaimTypes.SsoGroup, ssoGroupClaim));

                var ssoGroupCurrentLevelClaim = ssoGroupClaim + "|" + Service.SsoGroup.Id;
                claims.Add(new BaseIdentification(SsoClaimTypes.SsoGroupCurrentLevel, ssoGroupCurrentLevelClaim));

                var ssoGroupParentLevelClaim = ssoGroupClaim + "|" + Service.SsoGroup.SsoGroupParentId.ToInt();
                claims.Add(new BaseIdentification(SsoClaimTypes.SsoGroupParentLevel, ssoGroupParentLevelClaim));

                var ssoGroupOrderClaim = ssoGroupClaim + "|" + Service.SsoGroup.Order;
                claims.Add(new BaseIdentification(SsoClaimTypes.SsoGroupOrder, ssoGroupOrderClaim));

                if (Service.SsoGroup.SubSsoGroups != null)
                {
                    var ssoGroupSubLevelClaim = ssoGroupClaim + "|" + string.Join("|", Service.SsoGroup.SubSsoGroups.Select(ssg => ssg.Id));
                    claims.Add(new BaseIdentification(SsoClaimTypes.SsoGroupSubLevels, ssoGroupSubLevelClaim));
                }

                if (!string.IsNullOrEmpty(Service.NameOrDescription))
                {
                    //Creates claim values using Ids and adds to claims list
                    var serviceClaim = ssoGroupClaim + "|" + Service.NameOrDescription.Trim();
                    claims.Add(new BaseIdentification(SsoClaimTypes.SsoService, serviceClaim));

                    if (!string.IsNullOrEmpty(Service.Url))
                    {
                        //Creates claim values using Ids and adds to claims list
                        var serviceUrlClaim = serviceClaim + "|" + Service.Url.Trim();
                        claims.Add(new BaseIdentification(SsoClaimTypes.SsoServiceUrl, serviceUrlClaim));
                    }
                }
            }

            return claims;
        }
    }
}