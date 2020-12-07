using System.Collections.Generic;

namespace Zitadel.Authentication.Options
{
    /// <summary>
    /// Options for the Zitadel authentication handler.
    /// Those options are used for the handler only when it is used
    /// with AddZitadelAuthenticationHandler().
    /// </summary>
    public class ZitadelHandlerOptions
    {
        /// <summary>
        /// Defines the authority for the token.
        /// This defaults to <see cref="ZitadelDefaults.Issuer"/>.
        /// </summary>
        public string Authority { get; set; } = ZitadelDefaults.Issuer;

        /// <summary>
        /// The client id for the token verification.
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Defines if the received audiences should be validated or not.
        /// </summary>
        public bool ValidateAudience { get; set; } = true;

        /// <summary>
        /// List of valid audiences to check for.
        /// If <see cref="ValidateAudience"/> is `true` and this
        /// list is not defined, it defaults to a list with one entry.
        /// The entry is the value of <see cref="ClientId"/>.
        /// </summary>
        public IEnumerable<string>? ValidAudiences { get; set; }

        /// <summary>
        /// The issuer for the validation.
        /// </summary>
        public string Issuer { get; set; } = ZitadelDefaults.Issuer;

        /// <summary>
        /// The well-known discovery endpoint. This is used for opaque token
        /// validation to determine the user-info endpoint via discovery document.
        /// </summary>
        public string DiscoveryEndpoint { get; set; } = ZitadelDefaults.DiscoveryEndpoint;

        /// <summary>
        /// The primary domain - if any - that the user must belong to.
        /// </summary>
        public string? PrimaryDomain { get; set; }
    }
}
