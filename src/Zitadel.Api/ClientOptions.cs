using System.Collections.Generic;
using Zitadel.Authentication;
using Zitadel.Authentication.Credentials;

namespace Zitadel.Api
{
    /// <summary>
    /// Options for an API client.
    /// </summary>
    public class ClientOptions
    {
        /// <summary>
        /// The API endpoint for the client. This will be the base url for the api calls.
        /// </summary>
#if NET5_0_OR_GREATER
        public string Endpoint { get; init; } = ZitadelDefaults.ZitadelApiEndpoint;
#elif NETCOREAPP3_1_OR_GREATER
        public string Endpoint { get; set; } = ZitadelDefaults.ZitadelApiEndpoint;
#endif

        /// <summary>
        /// The organizational context in the API. This essentially defines the "x-zitadel-orgid" header value
        /// which provides the api with the orgId that the API call will be executed in.
        /// This may be overwritten for specific calls.
        /// </summary>
#if NET5_0_OR_GREATER
        public string Organization { get; init; } = string.Empty;
#elif NETCOREAPP3_1_OR_GREATER
        public string Organization { get; set; } = string.Empty;
#endif

        /// <summary>
        /// Authentication token for the client. This field may not be used in conjunction with
        /// <see cref="ServiceAccountAuthentication"/>. Use this field to explicitly set the
        /// Bearer token that will be transmitted to the API. If no authentication method is set,
        /// each call must attach the authorization header.
        /// </summary>
#if NET5_0_OR_GREATER
        public string? Token { get; init; }
#elif NETCOREAPP3_1_OR_GREATER
        public string? Token { get; set; }
#endif

        /// <summary>
        /// Service Account authentication method. If this field is set, the API calls are
        /// automatically authenticated with a <see cref="ServiceAccount"/> and the corresponding
        /// <see cref="ServiceAccount.AuthOptions"/>. This will renew the access token if it is
        /// expired.
        /// </summary>
#if NET5_0_OR_GREATER
        public (ServiceAccount Account, ServiceAccount.AuthOptions AuthOptions)? ServiceAccountAuthentication
        {
            get;
            init;
        }
#elif NETCOREAPP3_1_OR_GREATER
        public (ServiceAccount Account, ServiceAccount.AuthOptions AuthOptions)? ServiceAccountAuthentication
        {
            get;
            set;
        }
#endif

        /// <summary>
        /// List of additional arbitrary headers that are attached to each call.
        /// </summary>
#if NET5_0_OR_GREATER
        public IDictionary<string, string>? AdditionalHeaders { get; init; }
#elif NETCOREAPP3_1_OR_GREATER
        public IDictionary<string, string>? AdditionalHeaders { get; set; }
#endif
    }
}
