using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zitadel.Authentication;

namespace Zitadel.Spa.Dev
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services
                .AddAuthorization()
                .AddAuthentication()
                .AddZitadelAuthenticationHandler(
                    "ZitadelAuthHandlerJWT",
                    o => o.ClientId = "84891356119558811@zitadel_net")
                .AddZitadelAuthenticationHandler(
                    "ZitadelAuthHandlerJWTHostedDomain",
                    o =>
                    {
                        o.ClientId = "84891356119558811@zitadel_net";
                        o.PrimaryDomain = "smartive.zitadel.ch";
                    })
                .AddZitadelAuthenticationHandler(
                    "ZitadelAuthHandlerBearer",
                    o => o.ClientId = "84891241816386203@zitadel_net")
                .AddZitadelAuthenticationHandler(
                    "ZitadelAuthHandlerBearerHostedDomain",
                    o =>
                    {
                        o.ClientId = "84891241816386203@zitadel_net";
                        o.PrimaryDomain = "smartive.zitadel.ch";
                    });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints => { endpoints.MapControllers(); });
        }
    }
}
