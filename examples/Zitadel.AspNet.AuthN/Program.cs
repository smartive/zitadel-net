using Microsoft.AspNetCore.Identity;

using Zitadel.Authentication;
using Zitadel.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services
    .AddAuthorization()
    .AddAuthentication(ZitadelDefaults.AuthenticationScheme)
    .AddZitadel(
        o =>
        {
            o.Authority = "https://zitadel-libraries-l8boqa.zitadel.cloud/";
            o.ClientId = "170088295403946241@library";
            o.SignInScheme = IdentityConstants.ExternalScheme;
        })
    .AddExternalCookie()
    .Configure(
        o =>
        {
            o.Cookie.HttpOnly = true;
            o.Cookie.IsEssential = true;
            o.Cookie.SameSite = SameSiteMode.None;
            o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });

await app.RunAsync();
