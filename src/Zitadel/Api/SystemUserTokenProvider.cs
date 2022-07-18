using System.Security.Cryptography;
using System.Text;
using Jose;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Zitadel.Api;

internal class SystemUserTokenProvider : ITokenProvider
{
    private readonly Handler _handler;

    public SystemUserTokenProvider(string userId, string apiKey, string issuer) =>
        _handler = new(userId, apiKey, issuer);

    DelegatingHandler ITokenProvider.CreateHandler() => _handler;

    private class Handler : DelegatingHandler
    {
        private readonly string _issuer;
        private readonly string _userId;
        private readonly string _apiKey;
        private string? _token;

        public Handler(string issuer, string userId, string apiKey)
        {
            _issuer = issuer;
            _userId = userId;
            _apiKey = apiKey;
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

            _token ??= await CreateTokenAsync();
            request.Headers.Authorization = new("Bearer", _token);
            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string> CreateTokenAsync()
        {
            var bytes = Encoding.UTF8.GetBytes(_apiKey);
            await using var ms = new MemoryStream(bytes);
            using var sr = new StreamReader(ms);
            var pemReader = new PemReader(sr);

            if (pemReader.ReadObject() is not AsymmetricCipherKeyPair keyPair)
            {
                throw new("RSA Keypair could not be read.");
            }

            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(DotNetUtilities.ToRSAParameters(keyPair.Private as RsaPrivateCrtKeyParameters));

            return JWT.Encode(
                new Dictionary<string, object>
                {
                    { "iss", _userId },
                    { "sub", _userId },
                    { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
                    { "exp", (DateTimeOffset.UtcNow + TimeSpan.FromHours(1)).ToUnixTimeSeconds() },
                    { "aud", _issuer.TrimEnd('/') },
                },
                rsa,
                JwsAlgorithm.RS256);
        }
    }
}
