using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Alcoa.Framework.Domain.Entity
{
    public class EmailGatewayLog : Base, IBaseDomain
    {
        public EmailGatewayLog()
        {
            Validation = new BaseValidation();

            CurrentCode = string.Empty;
            JobSequence = default(int);
            JobType = string.Empty;
            ApplicationCode = string.Empty;
            Login = string.Empty;
            MessageCode = string.Empty;
            MessageDescription = string.Empty;
            Subject = string.Empty;
            To = string.Empty;
            StartDateTime = default(DateTime?);
            CreationDateTime = default(DateTime?);
        }

        public string CurrentCode { get; set; }

        public string JobType { get; set; }

        public int JobSequence { get; set; }

        public string ApplicationCode { get; set; }

        public string Login { get; set; }

        public string MessageCode { get; set; }

        public string MessageDescription { get; set; }

        public string Subject { get; set; }

        public string To { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? CreationDateTime { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(CurrentCode) && 
                string.IsNullOrEmpty(ApplicationCode) && 
                JobSequence <= default(int))
                Validation.Results.Add(new ValidationResult("Error: Invalid e-mail log object or was not found."));
        }

        #endregion
    }
}