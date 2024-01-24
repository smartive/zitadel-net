using FluentAssertions;

using Grpc.Core;

using Xunit;

using Zitadel.Api;

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
}
