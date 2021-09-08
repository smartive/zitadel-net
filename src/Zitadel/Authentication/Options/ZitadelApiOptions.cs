using System.Collections.Generic;
using Zitadel.Authentication.Credentials;

namespace Zitadel.Authentication.Options
{
    /// <summary>
    /// Options for the Zitadel API handler.
    /// Those options are used for the handler only when it is used
    /// with AddZitadelApi().
    /// </summary>
    public class ZitadelApiOptions
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
        /// If the API application uses Basic Authentication to authenticate
        /// itself against the IAM API, use this property to provide the credentials.
        /// Either <see cref="BasicAuthCredentials"/> or <see cref="JwtProfileKey"/>
        /// must be set. <see cref="JwtProfileKey"/> takes precedence if both fields are set.
        /// </summary>
        public BasicAuthentication? BasicAuthCredentials { get; set; }

        /// <summary>
        /// Correlates with <see cref="JwtProfileKey"/>. If the API application uses
        /// a private key JWT (recommended), this property can be set to pass the
        /// application object itself instead of a key path or key content.
        /// </summary>
        public Application? JwtProfile { get; set; }

        /// <summary>
        /// If the API application uses a private key JWT (recommended) to authenticate
        /// itself against the IAM API, use this property to provide the key information.
        /// Either use <see cref="JwtPrivateKeyPath"/> to provide a filepath to
        /// the json key, or an <see cref="JwtPrivateKeyContent"/> to provide the
        /// already loaded string content of a key.
        /// </summary>
        public JwtPrivateKey? JwtProfileKey { get; set; }

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
