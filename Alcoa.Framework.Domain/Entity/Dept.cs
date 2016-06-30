using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alcoa.Framework.Domain.Entity
{
    public class Dept : BaseMultiLanguageDescription, IBaseDomain
    {
        public Dept()
        {
            Validation = new BaseValidation();
        }

        public string DeptId { get; set; }

        public BaseValidation Validation { get; set; } 

        public void Validate()
        {
            if (string.IsNullOrEmpty(DeptId))
                Validation.Results.Add(new ValidationResult("Error: Id property can't be null"));
        }
    }
}