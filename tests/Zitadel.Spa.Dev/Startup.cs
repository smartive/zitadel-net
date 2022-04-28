using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zitadel.Authentication;
using Zitadel.Authentication.Credentials;
using Zitadel.Authorization;

namespace Zitadel.Spa.Dev
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            const string basicAuthClientId = "97767109911765031@zitadel_net";
            const string clientSecret = "NDNDmK5c1rGFElanXFxO61mcBKdonjdK7m7tH6sYUR0NlMKbztSgrrnRJGlwCkp8";
            const string jwtAuthClientId = "97767083403766481@zitadel_net";
            const string zitadelProjectId = "84856448403694484";

            services.AddControllers();
            services
                .AddAuthorization(
                    o => o.AddZitadelOrganizationRolePolicy("ZitadelUser", "69234230193872955", "Admin", "User"))
                .AddAuthentication()
                .AddJwtBearer()
                .AddZitadelApi(
                    "ZitadelApiJWT",
                    o =>
                    {
                        // Api auth with basic auth
                        o.ClientId = basicAuthClientId;
                        o.BasicAuthCredentials = new(basicAuthClientId, clientSecret);
                        o.ValidAudiences = new[] { zitadelProjectId };
                    })
                .AddZitadelApi(
                    "ZitadelApiJWTHostedDomain",
                    o =>
                    {
                        // Api auth with JWT profile
                        o.ClientId = jwtAuthClientId;
                        o.JwtProfileKey = new JwtPrivateKeyPath("./jwtProfileKey.json");
                        o.ValidAudiences = new[] { zitadelProjectId };
                        o.PrimaryDomain = "smartive.zitadel.ch";
                    })
                .AddZitadelApi(
                    "ZitadelApiBearer",
                    o =>
                    {
                        // Api auth with JWT profile
                        o.ClientId = jwtAuthClientId;
                        o.JwtProfileKey = new JwtPrivateKeyPath("./jwtProfileKey.json");
                        o.ValidAudiences = new[] { zitadelProjectId };
                    })
                .AddZitadelApi(
                    "ZitadelApiBearerHostedDomain",
                    o =>
                    {
                        // Api auth with basic auth
                        o.ClientId = basicAuthClientId;
                        o.BasicAuthCredentials = new(basicAuthClientId, clientSecret);
                        o.ValidAudiences = new[] { zitadelProjectId };
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
