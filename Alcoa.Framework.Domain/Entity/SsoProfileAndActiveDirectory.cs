using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Common.Enumerator;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alcoa.Framework.Domain.Entity
{
    public class SsoProfileAndActiveDirectory : Base, IBaseDomain
    {
        public SsoProfileAndActiveDirectory()
        {
            Validation = new BaseValidation();
        }

        public string ApplicationId { get; set; }

        public string ProfileId { get; set; }

        public SsoProfile Profile { get; set; }

        public string AdGroupId { get; set; }

        public ActiveDirectoryGroup AdGroup { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ApplicationId) &&
                string.IsNullOrEmpty(ProfileId) &&
                string.IsNullOrEmpty(AdGroupId))
                Validation.Results.Add(new ValidationResult("Error: Invalid SsoProfileAndActiveDirectory object or was not found."));
        }

        #endregion

        /// <summary>
        /// Get AD group and nested objects as Claims list
        /// </summary>
        public List<BaseIdentification> GetClaims()
        {
            //Creates claim values app, profile and AD group names
            if (!string.IsNullOrEmpty(AdGroup.NameOrDescription))
            {
                var adGroupClaim = ApplicationId + "|" + ProfileId + "|" + AdGroup.NameOrDescription.Trim();
                return new List<BaseIdentification> { new BaseIdentification(SsoClaimTypes.SsoActiveDirectoryGroup, adGroupClaim) };
            }

            return new List<BaseIdentification>();
        }
    }
}