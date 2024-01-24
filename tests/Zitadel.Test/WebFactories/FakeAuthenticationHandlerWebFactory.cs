using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Zitadel.Authentication;
using Zitadel.Extensions;

namespace Zitadel.Test.WebFactories;

public class FakeAuthenticationHandlerWebFactory : WebApplicationFactory<FakeAuthenticationHandlerWebFactory>
{
    #region Startup

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddAuthorization()
            .AddAuthentication(ZitadelDefaults.FakeAuthenticationScheme)
            .AddZitadelFake(
                options =>
                {
                    options.FakeZitadelId = "1234";
                    options.AdditionalClaims = new List<Claim> { new("foo", "bar"), };
                    options.Roles = new List<string> { "User" };
                });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapGet(
                    "/unauthed",
                    async context => { await context.Response.WriteAsJsonAsync(new Unauthed { Ping = "Pong" }); });
                endpoints.MapGet(
                        "/authed",
                        async context =>
                        {
                            await context.Response.WriteAsJsonAsync(
                                new Authed
                                {
                                    Ping = "Pong",
                                    AuthType = context.User.Identity?.AuthenticationType ?? string.Empty,
                                    UserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
                                    Claims = context.User.Claims.Select(
                                            c => new KeyValuePair<string, string>(c.Type, c.Value))
                                        .ToList(),
                                });
                        })
                    .RequireAuthorization();
            });
    }

    #endregion

    #region WebApplicationFactory

    protected override IHostBuilder CreateHostBuilder()
        => Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(
                builder => builder
                    .UseStartup<FakeAuthenticationHandlerWebFactory>());

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseContentRoot(Directory.GetCurrentDirectory());
        return base.CreateHost(builder);
    }

    #endregion

    #region Result Classes

    internal record Unauthed
    {
        public string Ping { get; init; } = null!;
    }

    internal record Authed
    {
        public string Ping { get; init; } = null!;

        public string AuthType { get; init; } = null!;

        public string UserId { get; init; } = null!;

        public List<KeyValuePair<string, string>> Claims { get; init; } = null!;
    }

    #endregion
}
