using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Alcoa.Framework.Domain.Entity
{
    public class EmailGateway : BaseEmail, IBaseDomain
    {
        public EmailGateway()
        {
            Validation = new BaseValidation();

            CurrentCode = string.Empty;
            JobSequence = default(int);
            ApplicationCode = string.Empty;
            Login = string.Empty;
            BodyComplement = string.Empty;
            AttachmentOne = string.Empty;
            AttachmentTwo = string.Empty;
            AttachmentThree = string.Empty;
            IsProcessed = string.Empty;
            IsEvent = string.Empty;
            AttemptNumber = default(short);
            CreationDateTime = default(DateTime?);
        }

        public string CurrentCode { get; set; }

        public int JobSequence { get; set; }

        public string ApplicationCode { get; set; }

        public string Login { get; set; }

        public string BodyComplement { get; set; }

        public string AttachmentOne { get; set; }

        public string AttachmentTwo { get; set; }

        public string AttachmentThree { get; set; }

        public string IsProcessed { get; set; }

        public string IsEvent { get; set; }

        public short AttemptNumber { get; set; }

        public DateTime? CreationDateTime { get; set; }

        public string SendResultMessage { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(CurrentCode) && 
                string.IsNullOrEmpty(ApplicationCode) && 
                JobSequence <= default(int))
                Validation.Results.Add(new ValidationResult("Error: Invalid e-mail object or was not found."));
        }

        #endregion
    }
}