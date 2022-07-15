using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zitadel.Authentication.Options;

namespace Zitadel.Authentication.Handler;

internal class LocalFakeZitadelHandler : AuthenticationHandler<LocalFakeZitadelSchemeOptions>
{
    private const string FakeAuthHeader = "x-zitadel-fake-auth";
    private const string FakeUserIdHeader = "x-zitadel-fake-user-id";

    public LocalFakeZitadelHandler(
        IOptionsMonitor<LocalFakeZitadelSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.Request.Headers.TryGetValue(FakeAuthHeader, out var value) && value == "false")
        {
            return Task.FromResult(AuthenticateResult.Fail($@"The {FakeAuthHeader} was set with value ""false""."));
        }

        var hasId = Context.Request.Headers.TryGetValue(FakeUserIdHeader, out var forceUserId);

        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, hasId ? forceUserId : Options.FakeZitadelOptions.FakeZitadelId),
                new("sub", hasId ? forceUserId : Options.FakeZitadelOptions.FakeZitadelId),
            }.Concat(Options.FakeZitadelOptions.AdditionalClaims)
            .Concat(Options.FakeZitadelOptions.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var identity = new ClaimsIdentity(claims, ZitadelDefaults.MockAuthenticationScheme);

        return Task.FromResult(
            AuthenticateResult.Success(
                new(new(identity), ZitadelDefaults.MockAuthenticationScheme)));
    }
}
