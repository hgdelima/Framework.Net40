using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Common.Enumerator;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alcoa.Framework.Domain.Entity
{
    public class ActiveDirectoryGroup : BaseActiveDirectoryGroup, IBaseDomain
    {
        public ActiveDirectoryGroup()
        {
            Validation = new BaseValidation();
        }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Id))
                Validation.Results.Add(new ValidationResult("Error: Id property can't be null"));

            if (string.IsNullOrEmpty(NameOrDescription))
                Validation.Results.Add(new ValidationResult("Error: NameOrDescription property can't be null"));
        }

        #endregion
    }
}