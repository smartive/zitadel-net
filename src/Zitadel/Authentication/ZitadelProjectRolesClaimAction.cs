using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace Zitadel.Authentication
{
    /// <summary>
    /// Claim action to fetch the citadel roles (if provided)
    /// from the JWT onto the user identity.
    /// </summary>
    internal class ZitadelProjectRolesClaimAction : ClaimAction
    {
        public ZitadelProjectRolesClaimAction()
            : base(ClaimTypes.Role, ClaimValueTypes.String)
        {
        }

        public override void Run(JsonElement userData, ClaimsIdentity identity, string issuer)
        {
            if (!userData.TryGetProperty(ZitadelDefaults.RoleClaimName, out var roles))
            {
                return;
            }

            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
            {
                roles.WriteTo(writer);
            }

            var decoded =
                JsonSerializer
                    .Deserialize<Dictionary<string, Dictionary<string, string>>>(bufferWriter.WrittenSpan);

            identity.AddClaims(
                decoded?.SelectMany(
                    role => role.Value
                        .Select(
                            org => new Claim(
                                ZitadelDefaults.OrganizationRoleClaimName(org.Key),
                                role.Key,
                                ClaimValueTypes.String,
                                issuer))
                        .Append(new(ClaimTypes.Role, role.Key, ClaimValueTypes.String, issuer))) ??
                Array.Empty<Claim>());
        }
    }
}
