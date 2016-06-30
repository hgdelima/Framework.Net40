using Alcoa.Entity.Entity;
using Alcoa.Framework.Common.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Common;
using Alcoa.Framework.Common.Enumerator;
using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Alcoa.Framework.Domain.Entity
{
    public class SsoApplication : BaseIdentification, IBaseDomain
    {
        public SsoApplication()
        {
            HomeUrl = string.Empty;
            Mnemonic = string.Empty;
            IsInactive = string.Empty;
            ToolType = string.Empty;
            Profiles = new List<SsoProfile>();
            Validation = new BaseValidation();
        }

        public virtual string HomeUrl { get; set; }

        public virtual string Mnemonic { get; set; }

        public virtual string IsInactive { get; set; }

        public virtual string ToolType { get; set; }

        public virtual List<SsoProfile> Profiles { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(IsInactive))
                Validation.Results.Add(new ValidationResult("Error: Invalid application object or was not found."));
        }

        #endregion

        /// <summary>
        /// Get as claims with all nested object claims
        /// </summary>
        public List<BaseIdentification> GetClaims()
        {
            //Initializes main claims
            var claims = new List<BaseIdentification>
            {
                new BaseIdentification(ClaimTypes.NameIdentifier, Id),
                new BaseIdentification(ClaimTypes.Name, Mnemonic)
            };

            //Add sso profiles, services and groups into claims list
            claims.AddRange(this.GetNestedClaims());

            return claims;
        }

        /// <summary>
        /// Get only nested object claims
        /// </summary>
        public List<BaseIdentification> GetNestedClaims()
        {
            var claims = new List<BaseIdentification> 
            { 
                new BaseIdentification(SsoClaimTypes.SsoApp, Id),
                new BaseIdentification(SsoClaimTypes.SsoAppMnemonic, Id + "|" + (Mnemonic ?? Id))
            };

            if (!string.IsNullOrEmpty(HomeUrl))
                claims.Add(new BaseIdentification(SsoClaimTypes.SsoAppHomeUrl, Id + "|" + HomeUrl.Trim()));

            //Add Sso profiles, services and groups into claims list
            claims.AddRange(Profiles.SelectMany(pro => pro.GetClaims()));

            return claims.Distinct(new EqualBy<BaseIdentification>((x, y) => x.Id == y.Id && x.NameOrDescription == y.NameOrDescription)).ToList();
        }

        /// <summary>
        /// Adds profiles and its dependencies
        /// </summary>
        public void AddProfilesAndDependencies(IEnumerable<SsoProfile> profiles)
        {
            var groupedProfiles = profiles
                .Where(p => p.ApplicationId == Id)
                .GroupBy(p => new { p.ApplicationId, p.Id })
                .Select(gp => gp.FirstOrDefault());

            if (groupedProfiles.Any())
            {
                Profiles.AddRange(groupedProfiles);

                Profiles = Profiles
                    .Where(p => p.ApplicationId == Id)
                    .GroupBy(p => new { p.ApplicationId, p.Id })
                    .Select(gp => gp.FirstOrDefault())
                    .ToList();
            }
        }

        /// <summary>
        /// Adds translations and its dependencies
        /// </summary>
        public void AddTranslations(IEnumerable<ApplicationTranslation> translations)
        {
            if (translations != default(IEnumerable<ApplicationTranslation>))
            {
                translations = translations
                    .Where(t => t.ApplicationId.ToLower() == Id.ToLower())
                    .ToList();

                var appTranslationType = translations.FirstOrDefault(tt => tt.TranslationType.ParameterName == CommonTranslationParameter.TRANS_TP_EXC_APPL.ToString());

                //Set translation for App name
                if (appTranslationType != default(ApplicationTranslation))
                {
                    var appName = translations
                        .FirstOrDefault(t => t.Key == Id && t.TranslationTypeId == appTranslationType.TranslationTypeId);

                    if (appName != default(ApplicationTranslation))
                        NameOrDescription = appName.NameOrDescription;
                }

                //Set profiles, services and groups translations
                Profiles.AsParallel().ForAll(pro =>
                {
                    if (pro != null)
                        pro.AddTranslations(translations);
                });
            }
        }
    }
}