# Authentication and Authorization

To directly integrate Zitadel into the ecosystem of
Microsoft and the provided webserver, this package extends the JWT Bearer
validator.

## Getting Started

To use Zitadel for authN, follow these steps:

- Create a new .net application with the generic host (e.g. ASP.Net Web Application)
- Install the package
- Configure the project on the selfhosted instance or [Zitadel Console](https://console.zitadel.ch)
- Configure the authentication
- Use the attributes or any other method of restricting access

### Create a .net application

Use the IDE of your choice or the command line to create
a new application.

```bash
dotnet new web
```

### Install the package

Install the package via nuget

```bash
dotnet add package Zitadel
```

### Configure Authentication

There exist several different methods to configure the authentication / authorization
for Zitadel.

- @"Zitadel.Authentication.AuthenticationBuilderExtensions.AddZitadel(Microsoft.AspNetCore.Authentication.AuthenticationBuilder,System.Action{Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectOptions})":
  Add the zitadel authentication. This only adds the auth scheme without any other configuration. Use
  this if you need to persist your tokens elsewhere or if you want to configure other mechanisms.
- @"Zitadel.Authentication.AuthenticationBuilderExtensions.AddZitadelWithSession(Microsoft.AspNetCore.Authentication.AuthenticationBuilder,System.Action{Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectOptions},System.Action{Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions})":
  Add zitadel with external application cookie support of Microsoft. This is useful, if you want
  to use zitadel for your razor page application to directly support session cookies.
- @"Zitadel.Authentication.AuthenticationBuilderExtensions.AddZitadelApi(Microsoft.AspNetCore.Authentication.AuthenticationBuilder,System.Action{Zitadel.Authentication.Options.ZitadelApiOptions})":
  Add an API only zitadel authentication. This is useful if your usecase is a single page application (SPA)
  and the dotnet application works as backend API for the SPA.
- @"Zitadel.Authentication.AuthenticationBuilderExtensions.AddFakeZitadel(Microsoft.AspNetCore.Authentication.AuthenticationBuilder,System.Action{Zitadel.Authentication.Options.LocalFakeZitadelOptions})":
  Useful for local development. This fakes a user and authenticates the user on each call.

As an example, your `Startup.cs` may look like this:

[!code-csharp[Startup.cs](../../tests/Zitadel.AuthN.Dev/Startup.cs?range=9-39&dedent=4&highlight=7-15,27-28)]

### Use Authorize Attribute

When your authentication is configure, you may use it in any appropriate way that dotnet provides.
As an example, in a Razor Page:

[!code-csharp[Authenticated.cshtml.cs](../../tests/Zitadel.AuthN.Dev/Pages/Authenticated.cshtml.cs?range=9-17&dedent=4&highlight=1)]
