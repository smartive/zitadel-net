using Zitadel.Credentials;

namespace Zitadel.Api;

/// <summary>
/// A <see cref="ITokenProvider"/> for gRPC clients that uses a
/// <see cref="ServiceAccount"/> to fetch an authenticated access token.
/// The token will be refreshed after the expiry date.
/// </summary>
/// <param name="Audience">The audience to authenticate against.</param>
/// <param name="ServiceAccount">The service account credentials for the authentication.</param>
/// <param name="AuthOptions">Specific authentication options for the service account.</param>
public record ServiceAccountTokenProvider(
    string Audience,
    ServiceAccount ServiceAccount,
    ServiceAccount.AuthOptions AuthOptions) : ITokenProvider
{
    DelegatingHandler ITokenProvider.CreateHandler() => new Handler(Audience, ServiceAccount, AuthOptions);

    private class Handler : DelegatingHandler
    {
        private static readonly TimeSpan ServiceTokenLifetime = TimeSpan.FromHours(12);

        private readonly string _audience;
        private readonly ServiceAccount _account;
        private readonly ServiceAccount.AuthOptions _options;

        private DateTime _tokenExpiryDate;
        private string? _token;

        public Handler(string audience, ServiceAccount account, ServiceAccount.AuthOptions options)
            : base(new HttpClientHandler())
        {
            _audience = audience;
            _account = account;
            _options = options;
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
            => SendAsync(request, cancellationToken).Result;

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
                _token = await _account.AuthenticateAsync(_audience, _options);
                _tokenExpiryDate = DateTime.UtcNow + ServiceTokenLifetime;
            }

            request.Headers.Authorization = new("Bearer", _token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
