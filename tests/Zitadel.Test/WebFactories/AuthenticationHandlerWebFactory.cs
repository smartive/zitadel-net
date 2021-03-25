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
using Zitadel.Authentication.Credentials;

namespace Zitadel.Test.WebFactories
{
    public class AuthenticationHandlerWebFactory : WebApplicationFactory<AuthenticationHandlerWebFactory>
    {
        #region Startup

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthorization()
                .AddAuthentication(ZitadelDefaults.ApiAuthenticationScheme)
                .AddZitadelApi(
                    o =>
                    {
                        o.ClientId = "100962076121492209@zitadel_net";
                        o.JwtProfileKey = new JwtPrivateKeyPath("api-application.json");
                        o.ValidAudiences = new[] { "84856448403694484" };
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
                                        AuthType = context.User.Identity?.AuthenticationType,
                                        UserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier),
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
                        .UseStartup<AuthenticationHandlerWebFactory>());

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            return base.CreateHost(builder);
        }

        #endregion

        #region Result Classes

        internal record Unauthed
        {
            public string Ping { get; init; }
        }

        internal record Authed
        {
            public string Ping { get; init; }

            public string AuthType { get; init; }

            public string UserId { get; init; }

            public List<KeyValuePair<string, string>> Claims { get; init; }
        }

        #endregion
    }
}
