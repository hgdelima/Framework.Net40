using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Alcoa.Framework.Common.Enumerator;

namespace Alcoa.Framework.Domain.Entity
{
    public class SsoServices : BaseIdentification, IBaseDomain
    {
        public SsoServices()
        {
            Validation = new BaseValidation();
        }

        public string ApplicationId { get; set; }

        public string Url { get; set; }

        public string ServiceType { get; set; }

        public string IsPrincipal { get; set; }

        public string IsFullScreen { get; set; }

        public string IsPublic { get; set; }

        public string IsDeepLink { get; set; }

        public string IsExternalProvider { get; set; }

        public string GroupId { get; set; }

        public SsoGroup SsoGroup { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(ApplicationId))
                Validation.Results.Add(new ValidationResult("Error: Invalid Service object or was not found."));
        }

        #endregion

        /// <summary>
        /// Adds translations and its dependencies
        /// </summary>
        public void AddTranslations(IEnumerable<ApplicationTranslation> translations)
        {
            if (translations != default(IEnumerable<ApplicationTranslation>))
            {
                translations = translations
                    .Where(t => t.ApplicationId.ToLower() == ApplicationId.ToLower())
                    .ToList();

                var serviceTranslationType = translations.FirstOrDefault(tt => tt.TranslationType.ParameterName == CommonTranslationParameter.TRANS_TP_EXC_SERVICO.ToString());
                var groupTranslationType = translations.FirstOrDefault(tt => tt.TranslationType.ParameterName == CommonTranslationParameter.TRANS_TP_EXC_GRP_EXB.ToString());

                //Set translation for service name
                if (serviceTranslationType != default(ApplicationTranslation))
                {
                    var serviceTranslation = translations
                        .FirstOrDefault(sn => sn.Key == Id && sn.TranslationTypeId == serviceTranslationType.TranslationTypeId);

                    if (serviceTranslation != default(ApplicationTranslation))
                        NameOrDescription = serviceTranslation.NameOrDescription;
                }

                //Set translation for group name
                if (groupTranslationType != default(ApplicationTranslation))
                {
                    var groupTranslation = translations
                        .FirstOrDefault(gt => gt.Key == GroupId && gt.TranslationTypeId == groupTranslationType.TranslationTypeId);

                    if (SsoGroup != null && groupTranslation != default(ApplicationTranslation))
                        SsoGroup.NameOrDescription = groupTranslation.NameOrDescription;
                }
            }
        }
    }
}