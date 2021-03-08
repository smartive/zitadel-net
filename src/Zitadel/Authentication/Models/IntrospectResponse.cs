using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Zitadel.Authentication.Models
{
    internal record IntrospectResponse
    {
        [JsonPropertyName("active")]
        public bool Active { get; init; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; init; } = string.Empty;

        [JsonPropertyName("sub")]
        public string Id { get; init; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("given_name")]
        public string GivenName { get; init; } = string.Empty;

        [JsonPropertyName("family_name")]
        public string FamilyName { get; init; } = string.Empty;

        [JsonPropertyName("nickname")]
        public string? Nickname { get; init; }

        [JsonPropertyName("gender")]
        public string? Gender { get; init; }

        [JsonPropertyName("username")]
        public string PreferredUsername { get; init; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; init; } = string.Empty;

        [JsonPropertyName("email_verified")]
        public bool EmailVerified { get; init; }

        [JsonPropertyName("locale")]
        public string? Locale { get; init; }

        [JsonPropertyName(ZitadelDefaults.RoleClaimName)]
        public Dictionary<string, Dictionary<string, string>>? Roles { get; init; }

        [JsonPropertyName(ZitadelDefaults.PrimaryDomainClaimName)]
        public string? PrimaryDomain { get; init; }

        public IEnumerable<Claim> CreateRoleClaims(string issuer)
        {
            if (Roles == null || Roles.Count == 0)
            {
                return Array.Empty<Claim>();
            }

            // First create the flat list of all possible roles
            // then attach organization specific roles.
            return Roles.SelectMany(
                role => role.Value
                    .Select(
                        org => new Claim(
                            ZitadelDefaults.OrganizationRoleClaimName(org.Key),
                            role.Key,
                            ClaimValueTypes.String,
                            issuer))
                    .Append(new(ClaimTypes.Role, role.Key, ClaimValueTypes.String, issuer)));
        }
    }
}
