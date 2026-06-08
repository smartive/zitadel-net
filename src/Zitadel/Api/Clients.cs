using Grpc.Core;
using Grpc.Net.Client;

using Zitadel.Action.V2;
using Zitadel.Admin.V1;
using Zitadel.Analytics.V2beta;
using Zitadel.Application.V2;
using Zitadel.Auth.V1;
using Zitadel.Authentication;
using Zitadel.Authorization.V2;
using Zitadel.Feature.V2;
using Zitadel.Idp.V2;
using Zitadel.Instance.V2;
using Zitadel.InternalPermission.V2;
using Zitadel.Management.V1;
using Zitadel.Oidc.V2;
using Zitadel.Org.V2;
using Zitadel.Project.V2;
using Zitadel.Saml.V2;
using Zitadel.Session.V2;
using Zitadel.Settings.V2;
using Zitadel.System.V1;
using Zitadel.User.V2;
using Zitadel.Webkey.V2;

namespace Zitadel.Api;

/// <summary>
/// Provides static methods to create and configure instances of gRPC API service clients for the ZITADEL API.
/// The clients are instantiated with default or specified options to ensure proper connectivity and authentication.
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
    /// Create a service client for the application service.
    /// </summary>
    /// <param name="options">Options for the client, including endpoint and authorization method.</param>
    /// <returns>The <see cref="Application.V2.ApplicationService.ApplicationServiceClient"/>.</returns>
    public static ApplicationService.ApplicationServiceClient ApplicationService(Options options) =>
        GetClient<ApplicationService.ApplicationServiceClient>(options);

    /// <summary>
    /// Create a service client for the project service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="Project.V2.ProjectService.ProjectServiceClient"/>.</returns>
    public static ProjectService.ProjectServiceClient ProjectService(Options options) =>
        GetClient<ProjectService.ProjectServiceClient>(options);

    /// <summary>
    /// Create a service client for the action service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="Action.V2.ActionService.ActionServiceClient"/>.</returns>
    public static ActionService.ActionServiceClient ActionService(Options options) =>
        GetClient<ActionService.ActionServiceClient>(options);

    /// <summary>
    /// Create a service client for the authorization service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="Authorization.V2.AuthorizationService.AuthorizationServiceClient"/>.</returns>
    public static AuthorizationService.AuthorizationServiceClient AuthorizationService(Options options) =>
        GetClient<AuthorizationService.AuthorizationServiceClient>(options);

    /// <summary>
    /// Create a service client for the feature service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="Feature.V2.FeatureService.FeatureServiceClient"/>.</returns>
    public static FeatureService.FeatureServiceClient FeatureService(Options options) =>
        GetClient<FeatureService.FeatureServiceClient>(options);

    /// <summary>
    /// Create a service client for the telemetry service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="TelemetryService.TelemetryServiceClient"/>.</returns>
    public static TelemetryService.TelemetryServiceClient TelemetryService(Options options) =>
        GetClient<TelemetryService.TelemetryServiceClient>(options);

    /// <summary>
    /// Create a service client for the SAML service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="Saml.V2.SAMLService.SAMLServiceClient"/>.</returns>
    public static SAMLService.SAMLServiceClient SAMLService(Options options) =>
        GetClient<SAMLService.SAMLServiceClient>(options);

    /// <summary>
    /// Create a service client for the web key service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="Webkey.V2.WebKeyService.WebKeyServiceClient"/>.</returns>
    public static WebKeyService.WebKeyServiceClient WebKeyService(Options options) =>
        GetClient<WebKeyService.WebKeyServiceClient>(options);

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

    /// <summary>
    /// Create a service client for the identity provider service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="IdentityProviderService.IdentityProviderServiceClient"/>.</returns>
    public static IdentityProviderService.IdentityProviderServiceClient IdentityProviderService(Options options) =>
        GetClient<IdentityProviderService.IdentityProviderServiceClient>(options);

    /// <summary>
    /// Create a service client for the instance service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="Instance.V2.InstanceService.InstanceServiceClient"/>.</returns>
    public static InstanceService.InstanceServiceClient InstanceService(Options options) =>
        GetClient<InstanceService.InstanceServiceClient>(options);

    /// <summary>
    /// Create a service client for the internal permission service.
    /// </summary>
    /// <param name="options">Options for the client like authorization method.</param>
    /// <returns>The <see cref="InternalPermission.V2.InternalPermissionService.InternalPermissionServiceClient"/>.</returns>
    public static InternalPermissionService.InternalPermissionServiceClient
        InternalPermissionService(Options options) =>
        GetClient<InternalPermissionService.InternalPermissionServiceClient>(options);

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
