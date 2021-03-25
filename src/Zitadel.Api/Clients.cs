using System;
using System.Net.Http;
using Caos.Zitadel.Admin.Api.V1;
using Caos.Zitadel.Auth.Api.V1;
using Caos.Zitadel.Management.Api.V1;
using Grpc.Core;
using Grpc.Net.Client;

namespace Zitadel.Api
{
    public static class Clients
    {
        public static AuthService.AuthServiceClient AuthService(ClientOptions options) =>
            GetClient<AuthService.AuthServiceClient>(options);

        public static AdminService.AdminServiceClient AdminService(ClientOptions options) =>
            GetClient<AdminService.AdminServiceClient>(options);

        public static ManagementService.ManagementServiceClient ManagementService(ClientOptions options) =>
            GetClient<ManagementService.ManagementServiceClient>(options);

        private static TClient GetClient<TClient>(ClientOptions options)
            where TClient : ClientBase<TClient>
        {
            var httpClient = new HttpClient();

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
