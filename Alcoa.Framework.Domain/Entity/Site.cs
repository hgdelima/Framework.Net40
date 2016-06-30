using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alcoa.Framework.Domain.Entity
{
    public class Site : BaseIdentification, IBaseDomain
    {
        public Site()
        {
            Validation = new BaseValidation();
        }

        public List<Lbc> Lbcs { get; set; }

        public List<Area> Areas { get; set; }

        public string IsActive { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public  void Validate()
        {
            if (string.IsNullOrEmpty(Id))
                Validation.Results.Add(new ValidationResult("Error: Id property can't be null"));
        }

        #endregion
    }
}