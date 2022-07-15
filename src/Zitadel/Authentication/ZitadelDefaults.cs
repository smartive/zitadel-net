using System.Security.Claims;
using Zitadel.Extensions;

namespace Zitadel.Authentication
{
    /// <summary>
    /// A set of default values for Zitadel themed authentication/authorization.
    /// </summary>
    public static class ZitadelDefaults
    {
        /// <summary>
        /// Default display name.
        /// </summary>
        public const string DisplayName = "ZITADEL";

        /// <summary>
        /// Default display name of the fake handler.
        /// </summary>
        public const string FakeDisplayName = "ZITADEL-Fake";

        /// <summary>
        /// Default authentication scheme name for AddZitadel().
        /// </summary>
        public const string AuthenticationScheme = "ZITADEL";

        /// <summary>
        /// Authentication scheme name for local fake provider.
        /// </summary>
        public const string MockAuthenticationScheme = "ZITADEL-Mock";

        /// <summary>
        /// Default callback path for local login redirection.
        /// </summary>
        public const string CallbackPath = "/signin-zitadel";

        /// <summary>
        /// Path to the well-known endpoint of the OIDC config.
        /// </summary>
        public const string DiscoveryEndpointPath = "/.well-known/openid-configuration";

        /// <summary>
        /// The name for the token claim (JWT or opaque via user-info endpoint)
        /// that contains rule allocations.
        /// </summary>
        public const string RoleClaimName = "urn:zitadel:iam:org:project:roles";

        /// <summary>
        /// The name for the token claim (JWT or opaque via user-info endpoint)
        /// that contains the scoped and used primary domain.
        /// </summary>
        public const string PrimaryDomainClaimName = "urn:zitadel:iam:org:domain:primary";

        /// <summary>
        /// Header which is used to provide context to grpc/rest api calls.
        /// </summary>
        public const string ZitadelOrgIdHeader = "x-zitadel-orgid";

        /// <summary>
        /// Constructor for organisation specific role claims.
        /// They are used to specify roles on a specific organization.
        /// Check for those roles with the policies added with
        /// <see cref="AuthorizationOptionsExtensions.AddZitadelOrganizationRolePolicy"/> or
        /// inside a method with
        /// <see cref="ClaimsPrincipalExtensions.IsInRole(ClaimsPrincipal,string,string[])"/>.
        /// </summary>
        /// <param name="orgId">The id of the organization.</param>
        /// <returns>A role name.</returns>
        public static string OrganizationRoleClaimName(string orgId) => $"urn:zitadel:iam:org:{orgId}:project:roles";
    }
}
