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
#if NET5_0_OR_GREATER
        public bool Active { get; init; }
#elif NETCOREAPP3_1_OR_GREATER
        public bool Active { get; set; }
#endif

        /// <summary>
        /// The client id associated with the token.
        /// </summary>
        [JsonPropertyName("client_id")]
#if NET5_0_OR_GREATER
        public string ClientId { get; init; } = string.Empty;
#elif NETCOREAPP3_1_OR_GREATER
        public string ClientId { get; set; } = string.Empty;
#endif

        /// <summary>
        /// The subject (i.e. resource) id of the token. This may be a userId, serviceAccountId or any
        /// other resource id in Zitadel.
        /// </summary>
        [JsonPropertyName("sub")]
#if NET5_0_OR_GREATER
        public string Id { get; init; } = string.Empty;
#elif NETCOREAPP3_1_OR_GREATER
        public string Id { get; set; } = string.Empty;
#endif

        /// <summary>
        /// The associated name.
        /// </summary>
        [JsonPropertyName("name")]
#if NET5_0_OR_GREATER
        public string Name { get; init; } = string.Empty;
#elif NETCOREAPP3_1_OR_GREATER
        public string Name { get; set; } = string.Empty;
#endif

        /// <summary>
        /// The associated given name.
        /// </summary>
        [JsonPropertyName("given_name")]
#if NET5_0_OR_GREATER
        public string GivenName { get; init; } = string.Empty;
#elif NETCOREAPP3_1_OR_GREATER
        public string GivenName { get; set; } = string.Empty;
#endif

        /// <summary>
        /// The associated family name.
        /// </summary>
        [JsonPropertyName("family_name")]
#if NET5_0_OR_GREATER
        public string FamilyName { get; init; } = string.Empty;
#elif NETCOREAPP3_1_OR_GREATER
        public string FamilyName { get; set; } = string.Empty;
#endif

        /// <summary>
        /// If set, the nickname.
        /// </summary>
        [JsonPropertyName("nickname")]
#if NET5_0_OR_GREATER
        public string? Nickname { get; init; }
#elif NETCOREAPP3_1_OR_GREATER
        public string? Nickname { get; set; }
#endif

        /// <summary>
        /// Gender.
        /// </summary>
        [JsonPropertyName("gender")]
#if NET5_0_OR_GREATER
        public string? Gender { get; init; }
#elif NETCOREAPP3_1_OR_GREATER
        public string? Gender { get; set; }
#endif

        /// <summary>
        /// The set preferred username.
        /// </summary>
        [JsonPropertyName("username")]
#if NET5_0_OR_GREATER
        public string PreferredUsername { get; init; } = string.Empty;
#elif NETCOREAPP3_1_OR_GREATER
        public string PreferredUsername { get; set; } = string.Empty;
#endif

        /// <summary>
        /// Email address.
        /// </summary>
        [JsonPropertyName("email")]
#if NET5_0_OR_GREATER
        public string Email { get; init; } = string.Empty;
#elif NETCOREAPP3_1_OR_GREATER
        public string Email { get; set; } = string.Empty;
#endif

        /// <summary>
        /// Defines if the email address is verified.
        /// </summary>
        [JsonPropertyName("email_verified")]
#if NET5_0_OR_GREATER
        public bool EmailVerified { get; init; }
#elif NETCOREAPP3_1_OR_GREATER
        public bool EmailVerified { get; set; }
#endif

        /// <summary>
        /// A locale language code.
        /// </summary>
        [JsonPropertyName("locale")]
#if NET5_0_OR_GREATER
        public string? Locale { get; init; }
#elif NETCOREAPP3_1_OR_GREATER
        public string? Locale { get; set; }
#endif

        /// <summary>
        /// Dictionary with associated roles to this token (if requested).
        /// The first key defines the role name, the second dictionary defines
        /// the org id and the org name.
        /// </summary>
        [JsonPropertyName(ZitadelDefaults.RoleClaimName)]
#if NET5_0_OR_GREATER
        public Dictionary<string, Dictionary<string, string>>? Roles { get; init; }
#elif NETCOREAPP3_1_OR_GREATER
        public Dictionary<string, Dictionary<string, string>>? Roles { get; set; }
#endif

        /// <summary>
        /// The primary domain (if requested).
        /// </summary>
        [JsonPropertyName(ZitadelDefaults.PrimaryDomainClaimName)]
#if NET5_0_OR_GREATER
        public string? PrimaryDomain { get; init; }
#elif NETCOREAPP3_1_OR_GREATER
        public string? PrimaryDomain { get; set; }
#endif

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
