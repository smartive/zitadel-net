using System.Collections.Generic;
using Zitadel.Authentication;
using Zitadel.Authentication.Credentials;

namespace Zitadel.Api
{
    /// <summary>
    /// Options for an API client.
    /// </summary>
    public record ClientOptions
    {
        /// <summary>
        /// The API endpoint for the client. This will be the base url for the api calls.
        /// </summary>
        public string Endpoint { get; init; } = ZitadelDefaults.ZitadelApiEndpoint;

        /// <summary>
        /// The organizational context in the API. This essentially defines the "x-zitadel-orgid" header value
        /// which provides the api with the orgId that the API call will be executed in.
        /// This may be overwritten for specific calls.
        /// </summary>
        public string Organization { get; init; } = string.Empty;

        /// <summary>
        /// Authentication token for the client. This field may not be used in conjunction with
        /// <see cref="ServiceAccountAuthentication"/>. Use this field to explicitly set the
        /// Bearer token that will be transmitted to the API. If no authentication method is set,
        /// each call must attach the authorization header.
        /// </summary>
        public string? Token { get; init; }

        /// <summary>
        /// Service Account authentication method. If this field is set, the API calls are
        /// automatically authenticated with a <see cref="ServiceAccount"/> and the corresponding
        /// <see cref="ServiceAccount.AuthOptions"/>. This will renew the access token if it is
        /// expired.
        /// </summary>
        public (ServiceAccount Account, ServiceAccount.AuthOptions AuthOptions)? ServiceAccountAuthentication
        {
            get;
            init;
        }

        /// <summary>
        /// List of additional arbitrary headers that are attached to each call.
        /// </summary>
        public IDictionary<string, string>? AdditionalHeaders { get; init; }
    }
}
