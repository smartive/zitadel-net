using AwesomeAssertions;

using Grpc.Core;

using Xunit;

using Zitadel.Api;
using Zitadel.Credentials;

namespace Zitadel.Test.Api;

public class ServiceAccountTokenProviderTest
{
    [Fact]
    public async Task Attaches_Valid_Token()
    {
        var client = Clients.AuthService(
            new(
                TestData.ApiUrl,
                ITokenProvider.ServiceAccount(TestData.ApiUrl, TestData.ServiceAccount, new() { ApiAccess = true })));
        var user = await client.GetMyUserAsync(new());
        user.Should().NotBeNull();
    }

    [Fact]
    public async Task Ignores_Request_If_Header_Already_Present()
    {
        var client = Clients.AuthService(
            new(
                TestData.ApiUrl,
                ITokenProvider.ServiceAccount(TestData.ApiUrl, TestData.ServiceAccount, new() { ApiAccess = true })));
        var meta = new Grpc.Core.Metadata { { "authorization", "Bearer foobar" } };
        await Assert.ThrowsAsync<RpcException>(async () => await client.GetMyUserAsync(new(), meta));
        meta.Get("authorization")?.Value.Should().Be("Bearer foobar");
    }

    [Fact]
    public async Task Reuses_Token_While_It_Is_Valid()
    {
        await using var server = await FakeOidcServer.StartAsync();
        server.ExpiresInSeconds = 43199;
        using var client = CreateClient(server);

        await client.GetStringAsync(server.ResourceEndpoint);
        await client.GetStringAsync(server.ResourceEndpoint);
        await client.GetStringAsync(server.ResourceEndpoint);

        server.TokenRequests.Should().Be(1);
    }

    [Fact]
    public async Task Renews_Token_When_The_Reported_Lifetime_Ran_Out()
    {
        await using var server = await FakeOidcServer.StartAsync();

        // A two second lifetime leaves a one second safety margin, so the token
        // has to be replaced after roughly a second - not after the hardcoded 12 hours.
        server.ExpiresInSeconds = 2;
        using var client = CreateClient(server);

        var first = await client.GetStringAsync(server.ResourceEndpoint);
        await Task.Delay(TimeSpan.FromSeconds(1.5));
        var second = await client.GetStringAsync(server.ResourceEndpoint);

        server.TokenRequests.Should().Be(2);
        second.Should().NotBe(first);
    }

    [Fact]
    public async Task Renews_Frequently_When_No_Lifetime_Is_Reported()
    {
        await using var server = await FakeOidcServer.StartAsync();
        server.ExpiresInSeconds = null;
        using var client = CreateClient(server);

        await client.GetStringAsync(server.ResourceEndpoint);

        // Without an "expires_in" the provider falls back to a short renewal interval
        // instead of trusting the token for hours.
        ServiceAccountTokenProvider.UnknownLifetimeFallback
            .Should()
            .BeLessThan(TimeSpan.FromHours(1));
        server.TokenRequests.Should().Be(1);
    }

    [Fact]
    public async Task Fetches_Only_One_Token_For_Concurrent_Calls()
    {
        await using var server = await FakeOidcServer.StartAsync();
        using var client = CreateClient(server);

        var bodies = await Task.WhenAll(
            Enumerable.Range(0, 20).Select(_ => client.GetStringAsync(server.ResourceEndpoint)));

        server.TokenRequests.Should().Be(1);
        bodies.Distinct().Should().HaveCount(1);
    }

    [Fact]
    public void Renewal_Time_Uses_The_Reported_Expiry_With_A_Safety_Margin()
    {
        var fetchedAt = new DateTimeOffset(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);
        var token = new ServiceAccount.AccessToken("token", fetchedAt.AddSeconds(43199));

        var renewAt = ServiceAccountTokenProvider.CalculateRenewalTime(token, fetchedAt);

        renewAt.Should().Be(fetchedAt.AddSeconds(43199) - ServiceAccountTokenProvider.MaxSafetyMargin);
        renewAt.Should().BeBefore(fetchedAt.AddSeconds(43199));
    }

    [Fact]
    public void Renewal_Time_Halves_Very_Short_Lifetimes()
    {
        var fetchedAt = new DateTimeOffset(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);
        var token = new ServiceAccount.AccessToken("token", fetchedAt.AddSeconds(10));

        var renewAt = ServiceAccountTokenProvider.CalculateRenewalTime(token, fetchedAt);

        // Subtracting the full margin would put the renewal time into the past.
        renewAt.Should().Be(fetchedAt.AddSeconds(5));
    }

    [Fact]
    public void Renewal_Time_Falls_Back_When_No_Expiry_Is_Known()
    {
        var fetchedAt = new DateTimeOffset(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);
        var token = new ServiceAccount.AccessToken("token", null);

        ServiceAccountTokenProvider.CalculateRenewalTime(token, fetchedAt)
            .Should()
            .Be(fetchedAt + ServiceAccountTokenProvider.UnknownLifetimeFallback);
    }

    [Fact]
    public void Renewal_Time_Does_Not_Trust_An_Already_Expired_Token()
    {
        var fetchedAt = new DateTimeOffset(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);
        var token = new ServiceAccount.AccessToken("token", fetchedAt.AddSeconds(-1));

        ServiceAccountTokenProvider.CalculateRenewalTime(token, fetchedAt)
            .Should()
            .BeBefore(fetchedAt);
    }

    private static HttpClient CreateClient(FakeOidcServer server)
    {
        ITokenProvider provider = new ServiceAccountTokenProvider(
            server.BaseUrl,
            TestData.ServiceAccount,
            new() { RequireHttps = false, DiscoveryEndpoint = server.DiscoveryEndpoint });

        return new(provider.CreateHandler());
    }
}
