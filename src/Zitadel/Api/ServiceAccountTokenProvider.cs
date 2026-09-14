using Zitadel.Credentials;

namespace Zitadel.Api;

/// <summary>
/// A <see cref="ITokenProvider"/> for gRPC clients that uses a
/// <see cref="ServiceAccount"/> to fetch an authenticated access token.
/// The token is renewed shortly before the expiry that the token endpoint
/// reported via "expires_in".
/// </summary>
/// <param name="Audience">The audience to authenticate against.</param>
/// <param name="ServiceAccount">The service account credentials for the authentication.</param>
/// <param name="AuthOptions">Specific authentication options for the service account.</param>
public record ServiceAccountTokenProvider(
    string Audience,
    ServiceAccount ServiceAccount,
    ServiceAccount.AuthOptions AuthOptions) : ITokenProvider
{
    /// <summary>
    /// A token is renewed this much ahead of its expiry, so that it does not die in flight
    /// because of clock skew or the latency of the call it is attached to. Tokens that live
    /// shorter than twice this margin use half of their lifetime instead.
    /// </summary>
    internal static readonly TimeSpan MaxSafetyMargin = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Renewal interval used when the token endpoint reports no "expires_in" value.
    /// It is deliberately short: without a reported lifetime the only safe assumption
    /// is that the token may be gone soon.
    /// </summary>
    internal static readonly TimeSpan UnknownLifetimeFallback = TimeSpan.FromMinutes(5);

    DelegatingHandler ITokenProvider.CreateHandler() => new Handler(Audience, ServiceAccount, AuthOptions);

    /// <summary>
    /// Determine when a freshly fetched token has to be replaced.
    /// </summary>
    /// <param name="token">The token that was just fetched.</param>
    /// <param name="fetchedAt">The point in time the token was requested.</param>
    /// <returns>
    /// The point in time at which the token has to be replaced. A token that arrived already
    /// expired yields a time in the past, which renews it on the next call.
    /// </returns>
    internal static DateTimeOffset CalculateRenewalTime(ServiceAccount.AccessToken token, DateTimeOffset fetchedAt)
    {
        if (token.ExpiresAt is not { } expiresAt)
        {
            return fetchedAt + UnknownLifetimeFallback;
        }

        var halfLifetime = (expiresAt - fetchedAt) / 2;
        return expiresAt - (halfLifetime < MaxSafetyMargin ? halfLifetime : MaxSafetyMargin);
    }

    private sealed class Handler(string audience, ServiceAccount account, ServiceAccount.AuthOptions options)
        : DelegatingHandler(new HttpClientHandler())
    {
        private readonly SemaphoreSlim _renewalLock = new(1, 1);

        /// <summary>
        /// The cached token and its renewal time as a single reference, so that a reader can
        /// never observe a token together with the renewal time of a different one.
        /// </summary>
        private CachedToken? _cachedToken;

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization == null)
            {
                // Fetching a token is asynchronous. Going through the thread pool keeps the wait
                // off a possible synchronization context, where it would deadlock.
                request.Headers.Authorization = new(
                    "Bearer",
                    Task.Run(() => GetTokenAsync(cancellationToken), cancellationToken).GetAwaiter().GetResult());
            }

            return base.Send(request, cancellationToken);
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization == null)
            {
                request.Headers.Authorization = new("Bearer", await GetTokenAsync(cancellationToken));
            }

            return await base.SendAsync(request, cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _renewalLock.Dispose();
            }

            base.Dispose(disposing);
        }

        private async Task<string> GetTokenAsync(CancellationToken cancellationToken)
        {
            if (Volatile.Read(ref _cachedToken) is { } cached && DateTimeOffset.UtcNow < cached.RenewAt)
            {
                return cached.Token;
            }

            // Only one caller fetches a token; the others wait and then use its result.
            await _renewalLock.WaitAsync(cancellationToken);
            try
            {
                if (Volatile.Read(ref _cachedToken) is { } current && DateTimeOffset.UtcNow < current.RenewAt)
                {
                    return current.Token;
                }

                var fetchedAt = DateTimeOffset.UtcNow;
                var token = await account.AuthenticateAsync(audience, options);
                Volatile.Write(ref _cachedToken, new(token.Token, CalculateRenewalTime(token, fetchedAt)));
                return token.Token;
            }
            finally
            {
                _renewalLock.Release();
            }
        }

        private sealed record CachedToken(string Token, DateTimeOffset RenewAt);
    }
}
