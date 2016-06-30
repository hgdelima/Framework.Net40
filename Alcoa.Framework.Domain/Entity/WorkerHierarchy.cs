using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alcoa.Framework.Domain.Entity
{
    public class WorkerHierarchy : Base, IBaseDomain
    {
        public WorkerHierarchy()
        {
            Validation = new BaseValidation();
        }

        public string ManagerId { get; set; }

        public virtual Worker Manager { get; set; }

        public string EmployeeId { get; set; }

        public virtual Worker Employee { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ManagerId))
                Validation.Results.Add(new ValidationResult("Error: Invalid worker hierarchy object or was not found."));
        }

        #endregion
    }
}