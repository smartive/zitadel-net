using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;
using Zitadel.Test.WebFactories;

namespace Zitadel.Test.Authentication
{
    public class ZitadelAuthenticationHandlerTest : IClassFixture<AuthenticationHandlerWebFactory>
    {
        private readonly AuthenticationHandlerWebFactory _factory;

        public ZitadelAuthenticationHandlerTest(AuthenticationHandlerWebFactory factory)
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
        public async Task Should_Return_Unauthorized_Without_Any_Token()
        {
            var client = _factory.CreateClient();
            var result = await client.GetAsync("/authed");
            result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact(Skip = "Until a solution for 'get an access token' is found.")]
        public async Task Should_Return_Data_With_Jwt_Token()
        {
            var client = _factory.CreateClient();
            var result = await client.GetAsync("/authed");
            result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }
    }
}
