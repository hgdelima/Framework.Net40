using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Common.Enumerator;
using Microsoft.IdentityModel.Claims;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Alcoa.Framework.Domain.Entity
{
    public class SsoProfile : BaseIdentification, IBaseDomain
    {
        public SsoProfile()
        {
            Validation = new BaseValidation();
            ProfilesAndActiveDirectories = new List<SsoProfileAndActiveDirectory>();
            ProfilesAndServices = new List<SsoProfileAndService>();
            ProfilesAndWorkers = new List<SsoProfileAndWorker>();
        }

        public string ApplicationId { get; set; }

        public string ProfileType { get; set; }

        public string IsPublic { get; set; }

        public string Order { get; set; }

        public List<SsoProfileAndActiveDirectory> ProfilesAndActiveDirectories { get; set; }

        public List<SsoProfileAndService> ProfilesAndServices { get; set; }

        public List<SsoProfileAndWorker> ProfilesAndWorkers { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ApplicationId))
                Validation.Results.Add(new ValidationResult("Error: ApplicationId property can't be null"));

            if (string.IsNullOrEmpty(Id))
                Validation.Results.Add(new ValidationResult("Error: Id property can't be null"));
        }

        #endregion

        /// <summary>
        /// Get profiles and nested objects as Claims list
        /// </summary>
        public List<BaseIdentification> GetClaims()
        {
            //Initializes claims list using app id and profile id
            var claims = new List<BaseIdentification> { new BaseIdentification(SsoClaimTypes.SsoProfile, ApplicationId + "|" + Id) };

            //Add AD groups
            if (ProfilesAndActiveDirectories.Count > 0)
                claims.AddRange(ProfilesAndActiveDirectories
                    .Where(pad => pad.AdGroup != null)
                    .SelectMany(pad => pad.GetClaims()));

            //Add Sso Groups and Services
            if (ProfilesAndServices.Count > 0)
                claims.AddRange(ProfilesAndServices
                    .Where(pas => pas.Service != null)
                    .SelectMany(pas => pas.GetClaims()));

            return claims;
        }

        /// <summary>
        /// Adds translations to profile and dependencies
        /// </summary>
        public void AddTranslations(IEnumerable<ApplicationTranslation> translations)
        {
            if (translations != default(IEnumerable<ApplicationTranslation>))
            {
                translations = translations
                    .Where(t => t.ApplicationId.ToLower() == ApplicationId.ToLower())
                    .ToList();

                var profileTranslationType = translations.FirstOrDefault(tt => tt.TranslationType.ParameterName == CommonTranslationParameter.TRANS_TP_EXC_GRUPO.ToString());

                //Set translation for profile name
                if (profileTranslationType != default(ApplicationTranslation))
                {
                    var profileTranslation = translations
                        .FirstOrDefault(pt => pt.Key == Id && pt.TranslationTypeId == profileTranslationType.TranslationTypeId);

                    if (profileTranslation != default(ApplicationTranslation))
                        NameOrDescription = profileTranslation.NameOrDescription;
                }

                //Set translations for nested Services and Groups
                ProfilesAndServices.AsParallel().ForAll(pas =>
                {
                    if (pas.Service != null)
                        pas.Service.AddTranslations(translations);
                });
            }
        }
    }
}