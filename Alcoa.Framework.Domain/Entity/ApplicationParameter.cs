using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alcoa.Framework.Domain.Entity
{
    public class ApplicationParameter : BaseDescription, IBaseDomain
    {
        public ApplicationParameter()
        {
            Validation = new BaseValidation();
        }

        public string ApplicationId { get; set; }

        public string ParameterName { get; set; }

        public string Content { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ApplicationId))
                Validation.Results.Add(new ValidationResult("Error: ApplicationId property can't be null"));

            if (string.IsNullOrEmpty(ParameterName))
                Validation.Results.Add(new ValidationResult("Error: ParameterName property can't be null"));

            if (string.IsNullOrEmpty(Content))
                Validation.Results.Add(new ValidationResult("Error: Content property can't be null"));
        }

        #endregion
    }
}