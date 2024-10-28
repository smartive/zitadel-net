using Grpc.Core;
using Grpc.Net.Client;

using Zitadel.Admin.V1;
using Zitadel.Auth.V1;
using Zitadel.Authentication;
using Zitadel.Management.V1;
using Zitadel.Oidc.V2;
using Zitadel.Org.V2;
using Zitadel.Session.V2;
using Zitadel.Settings.V2;
using Zitadel.System.V1;
using Zitadel.User.V2;

namespace Zitadel.Api;

/// <summary>
/// Helper class to instantiate (gRPC) api service clients for the ZITADEL API with correct settings.
/// All other versions are still available, but the latest version is used by default.
/// </summary>
public static class Clients
{
    /// <summary>
    /// Create a service client for the admin service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="Admin.V1.AdminService.AdminServiceClient"/>.</returns>
    public static AdminService.AdminServiceClient AdminService(Options options) =>
        GetClient<AdminService.AdminServiceClient>(options);

    /// <summary>
    /// Create a service client for the auth service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="Auth.V1.AuthService.AuthServiceClient"/>.</returns>
    public static AuthService.AuthServiceClient AuthService(Options options) =>
        GetClient<AuthService.AuthServiceClient>(options);

    /// <summary>
    /// Create a service client for the management service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="Management.V1.ManagementService.ManagementServiceClient"/>.</returns>
    public static ManagementService.ManagementServiceClient ManagementService(Options options) =>
        GetClient<ManagementService.ManagementServiceClient>(options);

    /// <summary>
    /// Create a service client for the oidc service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="OIDCService.OIDCServiceClient"/>.</returns>
    public static OIDCService.OIDCServiceClient OidcService(Options options) =>
        GetClient<OIDCService.OIDCServiceClient>(options);

    /// <summary>
    /// Create a service client for the organization service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="OrganizationService.OrganizationServiceClient"/>.</returns>
    public static OrganizationService.OrganizationServiceClient OrganizationService(Options options) =>
        GetClient<OrganizationService.OrganizationServiceClient>(options);

    /// <summary>
    /// Create a service client for the session service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="SessionService.SessionServiceClient"/>.</returns>
    public static SessionService.SessionServiceClient SessionService(Options options) =>
        GetClient<SessionService.SessionServiceClient>(options);

    /// <summary>
    /// Create a service client for the settings service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="SettingsService.SettingsServiceClient"/>.</returns>
    public static SettingsService.SettingsServiceClient SettingsService(Options options) =>
        GetClient<SettingsService.SettingsServiceClient>(options);

    /// <summary>
    /// Create a service client for the system service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="SystemService.SystemServiceClient"/>.</returns>
    public static SystemService.SystemServiceClient SystemService(Options options) =>
        GetClient<SystemService.SystemServiceClient>(options);

    /// <summary>
    /// Create a service client for the user service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="UserService.UserServiceClient"/>.</returns>
    public static UserService.UserServiceClient UserService(Options options) =>
        GetClient<UserService.UserServiceClient>(options);

    private static TClient GetClient<TClient>(Options options)
        where TClient : ClientBase<TClient>
    {
        var httpClient = options.TokenProvider != null ? new HttpClient(options.TokenProvider.CreateHandler()) : new();

        if (!string.IsNullOrWhiteSpace(options.Organization))
        {
            httpClient.DefaultRequestHeaders.Add(ZitadelDefaults.ZitadelOrgIdHeader, options.Organization);
        }

        if (options.AdditionalHeaders != null)
        {
            foreach (var (name, value) in options.AdditionalHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(name, value);
            }
        }

        var channel = GrpcChannel.ForAddress(
            options.Endpoint,
            new() { HttpClient = httpClient });
        var serviceType = typeof(TClient);

        return Activator.CreateInstance(serviceType, channel) as TClient ??
               throw new TypeInitializationException(
                   serviceType.FullName,
                   new($"Could not instantiate type {serviceType}"));
    }

    /// <summary>
    /// Creation options for a gRPC API client.
    /// </summary>
    /// <param name="Endpoint">Url on which the ZITADEL API is reachable.</param>
    /// <param name="TokenProvider">
    /// A token provider for the authentication and authorization.
    /// If omitted, each call must provide its own authentication token within the
    /// gRPC metadata.
    /// </param>
    public record Options(string Endpoint, ITokenProvider? TokenProvider)
    {
        /// <summary>
        /// The organizational context in the API. This essentially defines the "x-zitadel-orgid" header value
        /// which provides the api with the orgId that the API call will be executed in.
        /// This may be overwritten for specific calls. If omitted, no default header is added for
        /// the organization context and therefore must be set in each call.
        /// </summary>
        public string? Organization { get; init; }

        /// <summary>
        /// List of additional arbitrary headers that are attached to each call.
        /// </summary>
        public IDictionary<string, string>? AdditionalHeaders { get; init; }
    }
}
