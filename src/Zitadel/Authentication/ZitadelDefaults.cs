namespace Zitadel.Authentication
{
    public static class ZitadelDefaults
    {
        public const string DisplayName = "Zitadel";

        public const string AuthenticationScheme = "Zitadel";

        public const string HandlerAuthenticationScheme = "ZitadelAuthHandler";

        public const string Issuer = "https://issuer.zitadel.ch";

        public const string CallbackPath = "/signin-zitadel";

        public const string DiscoveryEndpoint = "https://issuer.zitadel.ch/.well-known/openid-configuration";

        public const string RoleClaimName = "urn:zitadel:iam:org:project:roles";

        public const string PrimaryDomainClaimName = "urn:zitadel:iam:org:domain:primary";
    }
}
