using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace Zitadel.Authentication.Validation
{
    public class ZitadelJwtTokenValidator : JwtSecurityTokenHandler
    {
        private readonly string? _primaryDomain;

        public ZitadelJwtTokenValidator(string? primaryDomain = null)
        {
            _primaryDomain = primaryDomain;
        }

        public override ClaimsPrincipal ValidateToken(
            string token,
            TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            var principal = base.ValidateToken(token, validationParameters, out validatedToken);

            if (_primaryDomain != null && !principal.HasClaim(ZitadelDefaults.PrimaryDomainClaimName, _primaryDomain))
            {
                // The user-token does not contain a primary domain claim
                // or it was the wrong value.
                return new ClaimsPrincipal();
            }

            var roles = principal.FindFirstValue(ZitadelDefaults.RoleClaimName);
            if (!string.IsNullOrWhiteSpace(roles))
            {
                var decoded =
                    JsonSerializer
                        .Deserialize<Dictionary<string, Dictionary<string, string>>>(roles);

                var securityToken = validatedToken;
                principal.AddIdentity(
                    new ClaimsIdentity(
                        decoded?.Keys
                            .Distinct()
                            .Select(
                                role => new Claim(
                                    ClaimTypes.Role,
                                    role,
                                    ClaimValueTypes.String,
                                    securityToken.Issuer)) ??
                        new Claim[0]));
            }

            return principal;
        }
    }
}
