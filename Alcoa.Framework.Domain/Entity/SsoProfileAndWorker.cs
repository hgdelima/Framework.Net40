using System;
using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Alcoa.Framework.Domain.Entity
{
    public class SsoProfileAndWorker : Base, IBaseDomain
    {
        public SsoProfileAndWorker()
        {
            Validation = new BaseValidation();
        }

        public string ApplicationId { get; set; }

        public string ProfileId { get; set; }

        public SsoProfile Profile { get; set; }

        public string WorkerOrEmployeeId { get; set; }

        public string AdminGroup { get; set; }

        public long RequestNumber { get; set; }

        public string Login { get; set; }

        public DateTime? DisableNotificationDate { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ApplicationId) &&
                string.IsNullOrEmpty(ProfileId) &&
                string.IsNullOrEmpty(WorkerOrEmployeeId))
                Validation.Results.Add(new ValidationResult("Error: Invalid SsoProfileAndWorker object or was not found."));
        }

        #endregion
    }
}