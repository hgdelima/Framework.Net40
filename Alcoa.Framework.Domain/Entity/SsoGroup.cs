using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alcoa.Framework.Domain.Entity
{
    public class SsoGroup : BaseIdentification, IBaseDomain
    {
        public SsoGroup()
        {
            Validation = new BaseValidation();
        }

        public string SsoGroupParentId { get; set; }

        public SsoGroup SsoGroupParent { get; set; }

        public string ApplicationId { get; set; }

        public int Order { get; set; }

        public List<SsoGroup> SubSsoGroups { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ApplicationId))
                Validation.Results.Add(new ValidationResult("Error: ApplicationId property can't be null"));

            if (string.IsNullOrEmpty(Id))
                Validation.Results.Add(new ValidationResult("Error: GroupId property can't be null"));
        }

        #endregion
    }
}