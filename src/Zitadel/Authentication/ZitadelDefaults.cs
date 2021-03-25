using Zitadel.Authentication.Credentials;
using Zitadel.Authorization;

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
        public const string DisplayName = "Zitadel";

        /// <summary>
        /// Default display name of the fake handler.
        /// </summary>
        public const string FakeDisplayName = "ZitadelLocalFake";

        /// <summary>
        /// Default authentication scheme name for AddZitadel() and
        /// AddZitadelWithSession().
        /// </summary>
        public const string AuthenticationScheme = "Zitadel";

        /// <summary>
        /// Authentication scheme name for local fake provider.
        /// </summary>
        public const string FakeAuthenticationScheme = "ZitadelLocalFake";

        /// <summary>
        /// Default authentication scheme name for AddZitadelApi().
        /// </summary>
        public const string ApiAuthenticationScheme = "ZitadelApi";

        /// <summary>
        /// Default online (non self hosted) Zitadel issuer.
        /// <a href="https://console.zitadel.ch">Zitadel.ch</a>
        /// </summary>
        public const string Issuer = "https://issuer.zitadel.ch";

        /// <summary>
        /// Default callback path for local login redirection.
        /// </summary>
        public const string CallbackPath = "/signin-zitadel";

        /// <summary>
        /// Default well-known discovery url for the online Zitadel instance.
        /// </summary>
        public const string DiscoveryEndpoint = "https://issuer.zitadel.ch/.well-known/openid-configuration";

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
        /// This is the project id for the zitadel API of <a href="https://console.zitadel.ch">Zitadel.ch</a>.
        /// This is needed for accessing the official zitadel API via grpc / rest endpoint
        /// when using a <see cref="ServiceAccount"/> to authenticate.
        /// </summary>
        public const string ZitadelApiProjectId = "69234237810729019";

        /// <summary>
        /// The endpoint (url) of the official zitadel API.
        /// </summary>
        public const string ZitadelApiEndpoint = "https://api.zitadel.ch";

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
        /// <see cref="ClaimsPrincipalExtensions.IsInRole(System.Security.Claims.ClaimsPrincipal,string,string[])"/>.
        /// </summary>
        /// <param name="orgId">The id of the organization.</param>
        /// <returns>A role name.</returns>
        public static string OrganizationRoleClaimName(string orgId) => $"urn:zitadel:iam:org:{orgId}:project:roles";
    }
}
