using Alcoa.Entity.Entity;
using Alcoa.Entity.Interfaces;
using Alcoa.Framework.Common;
using Alcoa.Framework.Common.Enumerator;
using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Alcoa.Framework.Domain.Entity
{
    public class Worker : BaseEmployee, IBaseDomain
    {
        public Worker()
        {
            Validation = new BaseValidation();
        }

        public virtual BudgetCode BudgetCode { get; set; }

        public virtual List<WorkerHierarchy> Manager { get; set; }

        public virtual List<WorkerHierarchy> Employees { get; set; }

        public virtual List<SsoApplication> Applications { get; set; }

        #region IBaseDomain

        public BaseValidation Validation { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(NameOrDescription))
                Validation.Results.Add(new ValidationResult("Error: Invalid worker object or was not found."));
        }

        #endregion

        /// <summary>
        /// Validate database user password
        /// </summary>
        public void ValidateUserCredential(string password)
        {
            //Validates user password if it was provided
            if (UserExtraInfo.AccountTypeName == AccountType.SSOUser.ToString())
            {
                if (string.IsNullOrEmpty(password))
                    Validation.Results.Add(new ValidationResult("Error: EncriptedPassword is null or empty"));

                if (LoginExpirationDate.HasValue && LoginExpirationDate < DateTime.Now)
                    Validation.Results.Add(new ValidationResult("Error: Login account expired"));

                if (string.IsNullOrEmpty(WebSignature))
                    Validation.Results.Add(new ValidationResult("Error: WebSignature is null or empty"));

                if (!string.IsNullOrEmpty(password) &&
                    !string.IsNullOrEmpty(WebSignature))
                {
                    password = CryptographHelper.RijndaelDecrypt(password, CommonConsts.CommonPassword);

                    //Creates the password to decrypt PrivateKeys
                    var prefix = CommonResource.GetString("PassNumbers") + CommonResource.GetString("PassSpecialChars");
                    var pass = prefix + CommonResource.GetString("PassText") + prefix;

                    var xmlPrivateKey = CryptographHelper.RijndaelDecrypt(WebSignatureRsaKey.PrivateKey.GetDescription(), pass);
                    var clearTextWebSignature = CryptographHelper.RsaDecrypt(WebSignature, xmlPrivateKey);

                    if (password != clearTextWebSignature)
                        Validation.Results.Add(new ValidationResult("Error: Password mismatch."));
                }
            }
        }

        /// <summary>
        /// Get as claims with all nested object claims
        /// </summary>
        public List<BaseIdentification> GetClaims()
        {
            //Initializes main claims
            var claims = new List<BaseIdentification>
            {
                new BaseIdentification(SsoClaimTypes.SsoId, Id),
                new BaseIdentification(SsoClaimTypes.SsoDomain, Domain ?? string.Empty),
                new BaseIdentification(ClaimTypes.NameIdentifier, NameOrDescription),
                new BaseIdentification(ClaimTypes.Name, NameOrDescription),
                new BaseIdentification(ClaimTypes.WindowsAccountName, Login),
                new BaseIdentification(ClaimTypes.Sid, (Sid ?? Login)),
                new BaseIdentification(ClaimTypes.Gender, (Gender ?? "M")),
                new BaseIdentification(ClaimTypes.Email, (Email ?? string.Empty)),
            };

            if (UserExtraInfo != null)
            {
                claims.Add(new BaseIdentification(ClaimTypes.GivenName, (UserExtraInfo.FirstName ?? string.Empty)));
                claims.Add(new BaseIdentification(ClaimTypes.Surname, (UserExtraInfo.LastName ?? string.Empty)));
                claims.Add(new BaseIdentification(ClaimTypes.Locality, (UserExtraInfo.Office ?? string.Empty)));
                claims.Add(new BaseIdentification(ClaimTypes.OtherPhone, ((UserExtraInfo.Phone ?? BranchLine) ?? string.Empty)));
            }
            else
                claims.Add(new BaseIdentification(ClaimTypes.OtherPhone, (BranchLine ?? string.Empty)));

            if (BirthDate.HasValue)
                claims.Add(new BaseIdentification(ClaimTypes.DateOfBirth, BirthDate.Value.ToString()));

            if (LoginExpirationDate.HasValue)
                claims.Add(new BaseIdentification(ClaimTypes.Expiration, LoginExpirationDate.Value.ToString()));

            //Add sso profiles, services and groups into claims list
            if (Applications != null)
            {
                var profileClaims = Applications
                    .Where(app => app != null)
                    .SelectMany(app => app.GetNestedClaims());

                claims.AddRange(profileClaims);
            }

            return claims;
        }
    }
}