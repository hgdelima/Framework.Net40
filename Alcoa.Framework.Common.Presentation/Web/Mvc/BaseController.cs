using Alcoa.Entity.Entity;
using Alcoa.Framework.Common.Enumerator;
using Alcoa.Framework.Common.Exceptions;
using Alcoa.Framework.Common.Presentation.GarService;
using Microsoft.IdentityModel.Claims;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Alcoa.Framework.Common.Presentation.Web.Mvc
{
    /// <summary>
    /// Base controller to be used to all MVC controllers
    /// </summary>
    public class BaseController : AbstractController
    {
        private BaseEmployee _appUser;
        protected BaseEmployee AppUser
        {
            get
            {

                try
                {
                    if (_appUser == null)
                    {
                        if (UserClaims == null)
                        {
                            _appUser = new BaseEmployee
                            {
                                Login = User.Identity.Name,
                                NameOrDescription = User.Identity.Name,
                            };
                        }
                        else
                        {
                            var emptyClaim = new Claim(string.Empty, string.Empty);

                            _appUser = new BaseEmployee
                            {
                                Id = UserClaims.FirstOrDefault(uc => uc.ClaimType == SsoClaimTypes.SsoId).Value,
                                Domain = UserClaims.FirstOrDefault(uc => uc.ClaimType == SsoClaimTypes.SsoDomain).Value,
                                NameOrDescription = UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Name).Value,
                                Email = UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Email).Value,
                                Login = UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.WindowsAccountName).Value,
                                Sid = UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Sid).Value,
                                Gender = UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Gender).Value,
                                BranchLine = UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.OtherPhone).Value,
                                BirthDate = (UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.DateOfBirth) ?? emptyClaim).Value.ToDateTimeNullable(),
                                LoginExpirationDate = (UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Expiration) ?? emptyClaim).Value.ToDateTimeNullable(),
                                UserExtraInfo = new BaseUserExtraInfo
                                {
                                    FirstName = (UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.GivenName) ?? emptyClaim).Value,
                                    LastName = (UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Surname) ?? emptyClaim).Value,
                                    Office = (UserClaims.FirstOrDefault(uc => uc.ClaimType == ClaimTypes.Locality) ?? emptyClaim).Value,
                                },
                            };
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw new PresentationException(CommonExceptionType.AppInitializationException, "Error when getting current User data. " + ex.GetAllExceptionMessages());
                }

                return _appUser;
            }
        }

        /// <summary>
        /// Initializes base and asbtract controller
        /// </summary>
        public BaseController()
        {
        }

        /// <summary>
        /// Create the header with default values for current application
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public override ActionResult GetHeader()
        {
            try
            {
                //Initialize default Header validations
                base.GetHeader();

                if (AppUser != null)
                {
                    Header.UserName = AppUser.NameOrDescription;
                    Header.UserGender = AppUser.Gender;
                    Header.UserEmail = AppUser.Email;
                    Header.UserExtensionLine = AppUser.BranchLine;
                }
            }
            catch (Exception ex)
            {
            }

            return PartialView("Header", Header);
        }
    }
}