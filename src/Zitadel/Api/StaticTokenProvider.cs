namespace Zitadel.Api;

/// <summary>
/// Static token provider that attaches the same token to all requests.
/// </summary>
/// <param name="Token">The access token to use.</param>
public record StaticTokenProvider(string Token) : ITokenProvider
{
    DelegatingHandler ITokenProvider.CreateHandler() => new Handler(Token);

    private class Handler : DelegatingHandler
    {
        private readonly string _token;

        public Handler(string token)
            : base(new HttpClientHandler()) => _token = token;

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

            request.Headers.Authorization = new("Bearer", _token);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
