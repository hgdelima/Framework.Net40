using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens;

namespace Alcoa.Framework.Common.Presentation.SSO
{
    /// <summary>
    /// Simple implementation of an issuer registy that returns the certificate issuer name or public key hash as an issuer
    /// </summary>
    public class SsoIssuerNameRegistry : IssuerNameRegistry
    {
        /// <summary>
        /// Gets the name of the issuer.
        /// </summary>
        /// <param name="securityToken">The security token.</param>
        public override string GetIssuerName(SecurityToken securityToken)
        {
            if (securityToken == null)
                throw new ArgumentNullException("securityToken");

            var x509Token = securityToken as X509SecurityToken;
            if (x509Token != null)
            {
                var issuer = x509Token.Certificate.Thumbprint;
                return issuer;
            }

            var rsaToken = securityToken as RsaSecurityToken;
            if (rsaToken != null)
            {
                var issuer = rsaToken.Rsa.ToXmlString(false);
                return issuer;
            }

            throw new SecurityTokenException(securityToken.GetType().FullName);
        }
    }
}
