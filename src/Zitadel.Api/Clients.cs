using System;
using System.Net.Http;
using Grpc.Core;
using Grpc.Net.Client;
using Zitadel.Admin.V1;
using Zitadel.Auth.V1;
using Zitadel.Authentication;
using Zitadel.Management.V1;

namespace Zitadel.Api
{
    /// <summary>
    /// Helper class to instantiate api service clients for the zitadel API with correct settings.
    /// </summary>
    public static class Clients
    {
        /// <summary>
        /// Create a service client for the auth service.
        /// </summary>
        /// <param name="options">Options for the client like authorization method.</param>
        /// <returns>The <see cref="Auth.V1.AuthService.AuthServiceClient"/>.</returns>
        public static AuthService.AuthServiceClient AuthService(ClientOptions options) =>
            GetClient<AuthService.AuthServiceClient>(options);

        /// <summary>
        /// Create a service client for the admin service.
        /// </summary>
        /// <param name="options">Options for the client like authorization method.</param>
        /// <returns>The <see cref="Admin.V1.AdminService.AdminServiceClient"/>.</returns>
        public static AdminService.AdminServiceClient AdminService(ClientOptions options) =>
            GetClient<AdminService.AdminServiceClient>(options);

        /// <summary>
        /// Create a service client for the management service.
        /// </summary>
        /// <param name="options">Options for the client like authorization method.</param>
        /// <returns>The <see cref="Management.V1.ManagementService.ManagementServiceClient"/>.</returns>
        public static ManagementService.ManagementServiceClient ManagementService(ClientOptions options) =>
            GetClient<ManagementService.ManagementServiceClient>(options);

        private static TClient GetClient<TClient>(ClientOptions options)
            where TClient : ClientBase<TClient>
        {
            var httpClient = options.Token == null && options.ServiceAccountAuthentication != null
                ? new HttpClient(
                    new ServiceAccountHttpHandler(
                        options.ServiceAccountAuthentication.Value.Account,
                        options.ServiceAccountAuthentication.Value.AuthOptions))
                : new HttpClient();

            httpClient.DefaultRequestHeaders.Add(ZitadelDefaults.ZitadelOrgIdHeader, options.Organization);

            if (options.Token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new("Bearer", options.Token);
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
                new GrpcChannelOptions { HttpClient = httpClient });
            var serviceType = typeof(TClient);

            return Activator.CreateInstance(serviceType, channel) as TClient ??
                   throw new($"Could not instantiate type {serviceType}");
        }
    }
}
