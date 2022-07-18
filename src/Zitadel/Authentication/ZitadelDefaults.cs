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
        public const string FakeAuthenticationScheme = "ZITADEL-Fake";

        /// <summary>
        /// Default callback path for local login redirection.
        /// </summary>
        public const string CallbackPath = "/signin-zitadel";

        /// <summary>
        /// Path to the well-known endpoint of the OIDC config.
        /// </summary>
        public const string DiscoveryEndpointPath = "/.well-known/openid-configuration";

        /// <summary>
        /// Header which is used to provide context to grpc/rest api calls.
        /// </summary>
        public const string ZitadelOrgIdHeader = "x-zitadel-orgid";
    }
}
