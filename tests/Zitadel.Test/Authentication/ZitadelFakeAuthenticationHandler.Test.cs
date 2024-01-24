using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;

using FluentAssertions;

using Xunit;

using Zitadel.Test.WebFactories;

namespace Zitadel.Test.Authentication;

public class ZitadelFakeAuthenticationHandler(FakeAuthenticationHandlerWebFactory factory)
    : IClassFixture<FakeAuthenticationHandlerWebFactory>
{
    [Fact]
    public async Task Should_Be_Able_To_Call_Unauthorized_Endpoint()
    {
        var client = factory.CreateClient();
        var result =
            await client.GetFromJsonAsync("/unauthed", typeof(AuthenticationHandlerWebFactory.Unauthed)) as
                AuthenticationHandlerWebFactory.Unauthed;
        result.Should().NotBeNull();
        result?.Ping.Should().Be("Pong");
    }

    [Fact]
    public async Task Should_Return_Unauthorized_With_The_Fail_Header()
    {
        var client = factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "/authed")
        {
            Headers = { { "x-zitadel-fake-auth", "false" } },
        };
        var result = await client.SendAsync(request);
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_Return_Authorized()
    {
        var client = factory.CreateClient();
        var result = await client.GetFromJsonAsync("/authed", typeof(AuthenticationHandlerWebFactory.Authed)) as
            AuthenticationHandlerWebFactory.Authed;
        result?.AuthType.Should().Be("ZITADEL-Fake");
        result?.UserId.Should().Be("1234");
        result?.Claims.Should().Contain(claim => claim.Key == ClaimTypes.Role && claim.Value == "User");
    }
}
