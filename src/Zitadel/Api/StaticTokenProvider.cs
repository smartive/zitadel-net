namespace Zitadel.Api;

/// <summary>
/// Static token provider that attaches the same token to all requests.
/// </summary>
/// <param name="Token">The access token to use.</param>
public record StaticTokenProvider(string Token) : ITokenProvider
{
    DelegatingHandler ITokenProvider.CreateHandler() => new Handler(Token);

    private sealed class Handler(string token) : DelegatingHandler(new HttpClientHandler())
    {
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken) =>
            SendAsync(request, cancellationToken).Result;

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization != null)
            {
                return base.SendAsync(request, cancellationToken);
            }

            request.Headers.Authorization = new("Bearer", token);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
