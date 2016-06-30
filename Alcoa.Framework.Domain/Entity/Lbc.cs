using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alcoa.Framework.Domain.Entity
{
    public class Lbc : BaseLbc, IBaseDomain
    {
        public Lbc()
        {
            Validation = new BaseValidation();
        }

        public string LbcId { get; set; }

        public List<Site> Sites { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(LbcId))
                Validation.Results.Add(new ValidationResult("Error: Id property can't be null"));
        }

        #endregion
    }
}