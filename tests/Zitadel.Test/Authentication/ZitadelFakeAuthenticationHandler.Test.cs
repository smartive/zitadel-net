using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;
using Zitadel.Test.WebFactories;

namespace Zitadel.Test.Authentication
{
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
            result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task Should_Return_Authorized()
        {
            var client = _factory.CreateClient();
            var result = await client.GetFromJsonAsync("/authed", typeof(AuthenticationHandlerWebFactory.Authed)) as
                AuthenticationHandlerWebFactory.Authed;
            result?.AuthType.Should().Be("ZitadelLocalFake");
            result?.UserId.Should().Be("1234");
            result?.Claims.Should().Contain(claim => claim.Key == ClaimTypes.Role && claim.Value == "User");
        }
    }
}
