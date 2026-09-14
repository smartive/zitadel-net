using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Zitadel.Test.Api;

/// <summary>
/// A minimal stand-in for a ZITADEL OIDC endpoint. It counts how often a token was
/// requested and controls the reported "expires_in", which lets the token provider
/// be tested without a real instance.
/// </summary>
internal sealed class FakeOidcServer : IAsyncDisposable
{
    private WebApplication? _app;
    private int _tokenRequests;
    private int _issuedTokens;

    private FakeOidcServer()
    {
    }

    /// <summary>The root url the server listens on.</summary>
    public string BaseUrl { get; private set; } = string.Empty;

    /// <summary>The discovery endpoint to hand to the service account auth options.</summary>
    public string DiscoveryEndpoint => $"{BaseUrl}/.well-known/openid-configuration";

    /// <summary>A resource endpoint that echoes the bearer token it received.</summary>
    public string ResourceEndpoint => $"{BaseUrl}/resource";

    /// <summary>How often the token endpoint was called.</summary>
    public int TokenRequests => Volatile.Read(ref _tokenRequests);

    /// <summary>The lifetime reported as "expires_in". Null omits the value entirely.</summary>
    public int? ExpiresInSeconds { get; set; } = 43199;

    public static async Task<FakeOidcServer> StartAsync()
    {
        var server = new FakeOidcServer();
        await server.StartInternalAsync();
        return server;
    }

    public async ValueTask DisposeAsync()
    {
        if (_app is null)
        {
            return;
        }

        await _app.StopAsync();
        await _app.DisposeAsync();
    }

    private async Task StartInternalAsync()
    {
        var builder = WebApplication.CreateSlimBuilder();
        builder.Logging.ClearProviders();
        builder.WebHost.UseUrls("http://127.0.0.1:0");

        var app = builder.Build();

        app.MapGet(
            "/.well-known/openid-configuration",
            () => Results.Json(
                new Dictionary<string, object>
                {
                    ["issuer"] = BaseUrl,
                    ["authorization_endpoint"] = $"{BaseUrl}/oauth/v2/authorize",
                    ["token_endpoint"] = $"{BaseUrl}/oauth/v2/token",
                    ["jwks_uri"] = $"{BaseUrl}/oauth/v2/keys",
                    ["response_types_supported"] = new[] { "code" },
                    ["subject_types_supported"] = new[] { "public" },
                    ["id_token_signing_alg_values_supported"] = new[] { "RS256" },
                }));

        app.MapGet(
            "/oauth/v2/keys",
            () => Results.Json(new Dictionary<string, object> { ["keys"] = Array.Empty<object>() }));

        app.MapPost("/oauth/v2/token", IssueToken);

        app.MapGet(
            "/resource",
            (HttpContext context) => Results.Text(
                context.Request.Headers.Authorization
                    .ToString()
                    .Replace("Bearer ", string.Empty, StringComparison.Ordinal)));

        await app.StartAsync();

        BaseUrl = app.Services
            .GetRequiredService<IServer>()
            .Features
            .Get<IServerAddressesFeature>()!
            .Addresses
            .First()
            .TrimEnd('/');

        _app = app;
    }

    private IResult IssueToken()
    {
        Interlocked.Increment(ref _tokenRequests);
        var serial = Interlocked.Increment(ref _issuedTokens);

        var body = new Dictionary<string, object>
        {
            ["access_token"] = $"fake-token-{serial}",
            ["token_type"] = "Bearer",
        };

        if (ExpiresInSeconds is { } expiresIn)
        {
            body["expires_in"] = expiresIn;
        }

        return Results.Json(body);
    }
}
