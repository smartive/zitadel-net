using System.Buffers;
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
        public override ClaimsPrincipal ValidateToken(
            string token,
            TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            var principal = base.ValidateToken(token, validationParameters, out validatedToken);

            // TODO: hosted domain check.

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
