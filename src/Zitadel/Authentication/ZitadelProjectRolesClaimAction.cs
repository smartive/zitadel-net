using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace Zitadel.Authentication
{
    public class ZitadelProjectRolesClaimAction : ClaimAction
    {
        private const string RolePropertyName = "urn:zitadel:iam:org:project:roles";

        public ZitadelProjectRolesClaimAction()
            : base(ClaimTypes.Role, ClaimValueTypes.String)
        {
        }

        public override void Run(JsonElement userData, ClaimsIdentity identity, string issuer)
        {
            if (!userData.TryGetProperty(RolePropertyName, out var roles))
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
                decoded?.Keys
                    .Distinct()
                    .Select(role => new Claim(ClaimTypes.Role, role, ClaimValueTypes.String, issuer)) ??
                new Claim[0]);
        }
    }
}
