# Zitadel Net

This is the `dotnet` library for authentication and authorization
for [Zitadel](https://zitadel.ch). The goal of the library is to
deliver a good developer experience like the packages from Microsoft
(e.g. `Microsoft.AspNetCore.Authentication.Google`).

## Prerequisities

To use this library efficiently you need:

1. Either:
   - Self-Hosted Zitadel instance
   - A user on [Zitadel.ch](https://console.zitadel.ch)
2. A created project on Zitadel IAM
3. An application in the said project with the correct oidc config
4. The client-id from said application

## Usage

After installation of the package, use it like described below.
Don't forget the `[Authorize]` attributes if you use them ;-)

### "Server-Side" Authorization

To add full managed authorization (meaning the authorization flow is
managed by the system at hand):

```csharp
// Startup.cs file
// -- snip --
services
    .AddAuthorization()
    .AddAuthentication(ZitadelDefaults.AuthenticationScheme)
    .AddZitadel(
        o =>
        {
            o.ClientId = "<<clientid>>";
            // The following line is not necessary, but
            // when set, it fetches the user information
            // from the issuer.
            o.GetClaimsFromUserInfoEndpoint = true;
        });
// -- snip --
```

### "Server-Side" Authorization w/ external cookie scheme

To add full managed authorization (meaning the authorization flow is
managed by the system at hand) with external cookie scheme (the session
is saved into an authorization cookie of asp.net with correct configuration)
use the following example:

```csharp
// Startup.cs file
// -- snip --
services
    .AddAuthorization()
    .AddAuthentication(ZitadelDefaults.AuthenticationScheme)
    .AddZitadelWithSession(
        o =>
        {
            o.ClientId = "<<clientid>>";
            // The following line is not necessary, but
            // when set, it fetches the user information
            // from the issuer.
            o.GetClaimsFromUserInfoEndpoint = true;
        });
// -- snip --
```

Note that for same site cookies, your development environment needs to run
on HTTPS!

### "Handler Only" Authorization

This is the case, when you create a WebApi that receives an
access token (JWT or opaque) from an SPA or some other application.

Since the authentication flow is not managed by the server, the
api only does validate and verify the given token.

When using JWT, the `JwtBearer` magic of Microsoft is used, with
the extension for Primary Domain. Otherwise, the opaque access
token is sent to the user-info endpoint of the issuer.

```csharp
// Startup.cs file
// -- snip --
services
    .AddAuthorization()
    .AddAuthentication(ZitadelDefaults.HandlerAuthenticationScheme)
    .AddZitadelAuthenticationHandler(
        o => o.ClientId = "<<clientid>>");
// -- snip --
```

And to add verification for a primary domain:

```csharp
// Startup.cs file
// -- snip --
services
    .AddAuthorization()
    .AddAuthentication(ZitadelDefaults.HandlerAuthenticationScheme)
    .AddZitadelAuthenticationHandler(
        o =>
        {
            o.ClientId = "<<clientid>>";
            o.PrimaryDomain = "foobar.ch";
        });
// -- snip --
```
