using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using Xunit;
using Zitadel.Test.WebFactories;

namespace Zitadel.Test.Authentication;

public class ZitadelFakeAuthenticationHandler : IClassFixture<FakeAuthenticationHandlerWebFactory>
{
    private readonly FakeAuthenticationHandlerWebFactory _factory;

    public ZitadelFakeAuthenticationHandler(FakeAuthenticationHandlerWebFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Should_Be_Able_To_Call_Unauthorized_Endpoint()
    {
        var client = _factory.CreateClient();
        var result =
            await client.GetFromJsonAsync("/unauthed", typeof(AuthenticationHandlerWebFactory.Unauthed)) as
                AuthenticationHandlerWebFactory.Unauthed;
        result.Should().NotBeNull();
        result?.Ping.Should().Be("Pong");
    }

    [Fact]
    public async Task Should_Return_Unauthorized_With_The_Fail_Header()
    {
        var client = _factory.CreateClient();
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
        var client = _factory.CreateClient();
        var result = await client.GetFromJsonAsync("/authed", typeof(AuthenticationHandlerWebFactory.Authed)) as
            AuthenticationHandlerWebFactory.Authed;
        result?.AuthType.Should().Be("ZITADEL-Fake");
        result?.UserId.Should().Be("1234");
        result?.Claims.Should().Contain(claim => claim.Key == ClaimTypes.Role && claim.Value == "User");
    }

    [Fact]
    public async Task Should_Trigger_Callback()
    {
        var client = _factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "/authed")
        {
            Headers = { { "x-zitadel-fake-user-id", "4321" } },
        };
        var result = await client.SendAsync(request);
        var content = await result.Content.ReadFromJsonAsync<AuthenticationHandlerWebFactory.Authed>();
        
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        content?.AuthType.Should().Be("ZITADEL-Fake");
        content?.UserId.Should().Be("4321");
        content?.Claims.Should().Contain(claim => claim.Key == "bar" && claim.Value == "foo");
        content?.Claims.Should().Contain(claim => claim.Key == ClaimTypes.Role && claim.Value == "Admin");
    }
}
