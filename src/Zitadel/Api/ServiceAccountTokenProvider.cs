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

    private sealed class Handler(string audience, ServiceAccount account, ServiceAccount.AuthOptions options)
        : DelegatingHandler(new HttpClientHandler())
    {
        private static readonly TimeSpan ServiceTokenLifetime = TimeSpan.FromHours(12);

        private DateTime _tokenExpiryDate;
        private string? _token;

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
                _token = await account.AuthenticateAsync(audience, options);
                _tokenExpiryDate = DateTime.UtcNow + ServiceTokenLifetime;
            }

            request.Headers.Authorization = new("Bearer", _token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
