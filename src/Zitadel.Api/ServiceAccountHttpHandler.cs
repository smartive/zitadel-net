using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Zitadel.Authentication.Credentials;

namespace Zitadel.Api
{
    internal class ServiceAccountHttpHandler : DelegatingHandler
    {
        private static readonly TimeSpan ServiceTokenLifetime = TimeSpan.FromHours(12);

        private readonly ServiceAccount _account;
        private readonly ServiceAccount.AuthOptions _options;

        private DateTime _tokenExpiryDate;
        private string? _token;

        public ServiceAccountHttpHandler(ServiceAccount account, ServiceAccount.AuthOptions options)
            : base(new HttpClientHandler())
        {
            _account = account;
            _options = options;
        }

#if NET5_0_OR_GREATER
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
            => SendAsync(request, cancellationToken).Result;
#endif

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization != null)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            // When the token is not fetched or it is expired, re-fetch a service account token.
            if (_token == null || _tokenExpiryDate < DateTime.UtcNow)
            {
                _token = await _account.AuthenticateAsync(_options);
                _tokenExpiryDate = DateTime.UtcNow + ServiceTokenLifetime;
            }

            request.Headers.Authorization = new("Bearer", _token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
