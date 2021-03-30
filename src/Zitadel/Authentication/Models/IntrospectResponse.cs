using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Zitadel.Authentication.Models
{
    /// <summary>
    /// The response of the introspect endpoint.
    /// </summary>
    public record IntrospectResponse
    {
        /// <summary>
        /// Defines if the token is active (== valid) or not.
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; init; }

        /// <summary>
        /// The client id associated with the token.
        /// </summary>
        [JsonPropertyName("client_id")]
        public string ClientId { get; init; } = string.Empty;

        /// <summary>
        /// The subject (i.e. resource) id of the token. This may be a userId, serviceAccountId or any
        /// other resource id in Zitadel.
        /// </summary>
        [JsonPropertyName("sub")]
        public string Id { get; init; } = string.Empty;

        /// <summary>
        /// The associated name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// The associated given name.
        /// </summary>
        [JsonPropertyName("given_name")]
        public string GivenName { get; init; } = string.Empty;

        /// <summary>
        /// The associated family name.
        /// </summary>
        [JsonPropertyName("family_name")]
        public string FamilyName { get; init; } = string.Empty;

        /// <summary>
        /// If set, the nickname.
        /// </summary>
        [JsonPropertyName("nickname")]
        public string? Nickname { get; init; }

        /// <summary>
        /// Gender.
        /// </summary>
        [JsonPropertyName("gender")]
        public string? Gender { get; init; }

        /// <summary>
        /// The set preferred username.
        /// </summary>
        [JsonPropertyName("username")]
        public string PreferredUsername { get; init; } = string.Empty;

        /// <summary>
        /// Email address.
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; init; } = string.Empty;

        /// <summary>
        /// Defines if the email address is verified.
        /// </summary>
        [JsonPropertyName("email_verified")]
        public bool EmailVerified { get; init; }

        /// <summary>
        /// A locale language code.
        /// </summary>
        [JsonPropertyName("locale")]
        public string? Locale { get; init; }

        /// <summary>
        /// Dictionary with associated roles to this token (if requested).
        /// The first key defines the role name, the second dictionary defines
        /// the org id and the org name.
        /// </summary>
        [JsonPropertyName(ZitadelDefaults.RoleClaimName)]
        public Dictionary<string, Dictionary<string, string>>? Roles { get; init; }

        /// <summary>
        /// The primary domain (if requested).
        /// </summary>
        [JsonPropertyName(ZitadelDefaults.PrimaryDomainClaimName)]
        public string? PrimaryDomain { get; init; }

        /// <summary>
        /// Create a list of .net (role) <see cref="Claim"/>s for a given issuer.
        /// </summary>
        /// <param name="issuer">The issuer to register.</param>
        /// <returns>An enumerable of role specific <see cref="Claim"/>s.</returns>
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
