using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;

using FluentAssertions;

using Xunit;

using Zitadel.Authentication;
using Zitadel.Test.WebFactories;

namespace Zitadel.Test.Authentication;

public class ZitadelAuthenticationHandlerTest(AuthenticationHandlerWebFactory factory)
    : IClassFixture<AuthenticationHandlerWebFactory>
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
    public async Task Should_Return_Unauthorized_Without_Any_Token()
    {
        var client = factory.CreateClient();
        var result = await client.GetAsync("/authed");
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_Return_Data_With_Token()
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new("Bearer", TestData.PersonalAccessToken);
        var result = await client.GetAsync("/authed");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Attach_Roles_To_Claims()
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new("Bearer", TestData.PersonalAccessToken);
        var result = await client.GetAsync("/authed");
        var parsed = await result.Content.ReadFromJsonAsync<AuthenticationHandlerWebFactory.Authed>();
        parsed?.Claims.Should().Contain(c => c.Key == ClaimTypes.Role);
        parsed?.Claims.Should().Contain(c => c.Key == ZitadelClaimTypes.OrganizationRole(TestData.OrgId));
        parsed?.Claims.Should().Contain(c => c.Value == "test-1");
        parsed?.Claims.Should().Contain(c => c.Value == "test-2");
    }
}
