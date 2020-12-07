using System.Collections.Generic;

namespace Zitadel.Authentication.Options
{
    public class ZitadelHandlerOptions
    {
        public string Authority { get; set; } = ZitadelDefaults.Issuer;

        public string ClientId { get; set; } = string.Empty;

        public bool ValidateAudience { get; set; } = true;

        /// <summary>
        /// List of valid audiences to check for.
        /// If <see cref="ValidateAudience"/> is `true` and this
        /// list is not defined, it defaults to a list with one entry.
        /// The entry is the value of <see cref="ClientId"/>.
        /// </summary>
        public IEnumerable<string>? ValidAudiences { get; set; }

        public string Issuer { get; set; } = ZitadelDefaults.Issuer;

        public string DiscoveryEndpoint { get; set; } = ZitadelDefaults.DiscoveryEndpoint;

        public string? HostedDomain { get; set; }
    }
}
