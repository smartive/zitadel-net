# ZITADEL

The ZITADEL.net library is a collection of tools for building web applications.
It supports easy access to the ZITADEL API as well as authentication handlers
for .NET web applications and web APIs.

## Credentials

There are three credentials that help with the access to
ZITADEL:

- "Application": used in web APIs to authenticate the relying party
- "BasicAuthentication": creating normal basic auth credentials
- "ServiceAccount": loads a service account json and authenticates against ZITADEL

The application supports creating a signed JWT token on behalf of the
application:

```csharp
var application = Application.LoadFromJsonString(
@"{
  ""type"": ""application"",
  ""keyId"": ""keyid"",
  ""key"": ""RSA KEY"",
  ""appId"": ""appid"",
  ""clientId"": ""client id""
}");
var jwt = await application.GetSignedJwtAsync("issuer");
```

The service account allows you to load a service account json and
authenticate against ZITADEL to fetch a valid access token:

```csharp
var serviceAccount = ServiceAccount.LoadFromJsonString(
    @"
{
  ""type"": ""serviceaccount"",
  ""keyId"": ""key id"",
  ""key"": ""RSA KEY"",
  ""userId"": ""user id""
}");
var token = await serviceAccount.AuthenticateAsync();
```

## Accessing the ZITADEL API

[ZITADEL.gRPC](../Zitadel.Grpc) provides the compiled proto files.
The ZITADEL library provides helper functions to create the four
types of "clients":

- `AuthClient`
- `AdminClient`
- `ManagementClient`
- `SystemClient`

The [ZITADEL docs](https://docs.zitadel.com/docs/apis/introduction) describe
the gRPC calls and how to use them.

As an example, one may use the `AuthClient` to fetch the user information.

### With a personal access token of a service account

```csharp
const string apiUrl = "https://zitadel-libraries-l8boqa.zitadel.cloud";
const string personalAccessToken = "TOKEN";
var client = Clients.AuthService(new(apiUrl, ITokenProvider.Static(personalAccessToken)));
var result = await client.GetMyUserAsync(new());
Console.WriteLine($"User: {result.User}");
```

### With a service account JWT profile

```csharp
const string apiProject = "PROJECT ID";
var serviceAccount = ServiceAccount.LoadFromJsonString(
@"{
  ""type"": ""serviceaccount"",
  ""keyId"": ""key id"",
  ""key"": ""RSA KEY"",
  ""userId"": ""user id""
}");
client = Clients.AuthService(
    new(
        apiUrl,
        ITokenProvider.ServiceAccount(
            serviceAccount,
            apiUrl,
            apiProject)));
result = await client.GetMyUserAsync(new());
Console.WriteLine($"User: {result.User}");
```

## Authentication in Web Apps

To authenticate ASP.NET web applications, use the `AddZitadel()` extension
method on the `IAuthenticationBuilder`. You will need an application
on a ZITADEL instance and a client ID.

```csharp
// -- snip --
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
// -- snip --
```

The example above allows an ASP.NET web application to authenticate against
ZITADEL and use the external cookie scheme to store the access token in
a secure cookie.

## Authentication in Web APIs

Authenticating web APIs is similar to authenticating web apps. In contrast to a
web application, the web API cannot hold a user session with an external application
cookie. Instead, web APIs use the introspection endpoint of ZITADEL to fetch information
about the presented access token (be it JWT or [opaque token](https://stackoverflow.com/questions/59158410/what-is-an-opaque-token)).
The authentication mechanism is based on the
[OAuth2Introspection](https://github.com/IdentityModel/IdentityModel.AspNetCore.OAuth2Introspection)
package of "IdentityModel".

In ZITADEL you may use two different authentication methods:

- Basic Auth
- JWT Profile

With basic auth, you need to use `client_id` and `client_secret`, and
with JWT profile, a special json is generated for you, that is required
to authenticate the web API against ZITADEL.

```csharp
builder.Services
    .AddAuthorization()
    .AddAuthentication()
    .AddZitadelIntrospection(
        o =>
        {
            o.Authority = "https://zitadel-libraries-l8boqa.zitadel.cloud/";
            o.ClientId = "170102032621961473@library";
            o.ClientSecret = "KNkKW8nx3rlEKOeHNUcPx80tZTP1uZTjJESfdA3kMEK7urhX3ChFukTMQrtjvG70";
        });
```

The code above uses basic authentication. You need to be sure that your API application
in ZITADEL is configured to use basic authentication.

Below, a JWT profile (application credential) is used to authenticate the web API.
Note that the client id is no longer required. Using JWT profile is the **recommended**
way to authenticate web APIs.

```csharp
builder.Services
    .AddAuthorization()
    .AddAuthentication()
    .AddZitadelIntrospection(
        o =>
        {
            o.Authority = "https://zitadel-libraries-l8boqa.zitadel.cloud";
            o.JwtProfile = Application.LoadFromJsonString("YOUR APPLICATION JSON");
        });
```

### Caching

The [OAuth2Introspection](https://github.com/IdentityModel/IdentityModel.AspNetCore.OAuth2Introspection)
supports caching of the access token for a configured amount of time. This reduces the load on
the issuer and allows faster requests for the same token. To enable caching, you need to configure
caching in the options of `AddZitadelIntrospection` and add an implementation of `IDistributedCache`.

## Faking / Mocking local Authentication

To enable local development or testing without a real world ZITADEL instance, you
may use the mocked authentication. It simply adds all provided claims to the
constructed identity and lets all calls pass as "authenticated".

You may send a request with two special headers to overwrite the behaviour per request:

- `x-zitadel-fake-auth`: If this header is set to "false", the request will return as "unauthenticated"
- `x-zitadel-fake-user-id`: If this header is set, the value of the header will be user as user ID.

To enable the fake authentication, simply use the `AddZitadelFake` extension method:

```csharp
builder.Services
    .AddAuthorization()
    .AddAuthentication()
    .AddZitadelFake(o =>
        {
            o.FakeZitadelId = "1337";
        });
```
