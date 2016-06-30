using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Alcoa.Framework.Domain.Entity
{
    public class ApplicationTranslation : BaseDescription, IBaseDomain
    {
        public ApplicationTranslation()
        {
            Validation = new BaseValidation();
        }

        public long TranslationId { get; set; }

        public string ApplicationId { get; set; }

        public string Key { get; set; }

        public int TranslationTypeId { get; set; }

        public ApplicationParameter TranslationType { get; set; }

        public string LanguageCultureName { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ApplicationId))
                Validation.Results.Add(new ValidationResult("Error: ApplicationId property can't be null"));

            if (string.IsNullOrEmpty(Key))
                Validation.Results.Add(new ValidationResult("Error: Key property can't be null"));

            if (string.IsNullOrEmpty(LanguageCultureName))
                Validation.Results.Add(new ValidationResult("Error: LanguageCultureName property can't be null"));
        }

        #endregion
    }
}