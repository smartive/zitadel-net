# Zitadel API

To use the Zitadel API as a client, you can use the provided `Zitadel.Api`
package to directly interact with the [gRPC](https://grpc.io/) api.

The package is generated from the official `*.proto` files of
[the Zitadel repository](https://github.com/zitadel/zitadel).

## How To Use

This usage example will show the way with a `ServiceAccount` authentication.
You may use any kind of authentication to obtain an access token which has
access to the Zitadel API Project (project id: `69234237810729019`).

You'll find many of those values in @"Zitadel.Authentication.ZitadelDefaults".

The general usage with a `ServiceAccount` works as follows:

1. Create the Service Account on the [zitadel console](https://console.zitadel.ch)
2. Give the correct access rights to the account
3. Create the access key (jwt profile private rsa key)
4. Load the service account
5. Create an API service client
6. ...
7. Profit!

This may look as seen in `Program.cs` of the tests project "Zitadel.Api.Access.Dev".

```csharp
// no.. this key is not activated anymore ;-)
var sa = await ServiceAccount.LoadFromJsonFileAsync("./service-account.json");
var api = Clients.ManagementService(
    new()
    {
        // Which api endpoint (self hosted or public)
        Endpoint = ZitadelDefaults.ZitadelApiEndpoint,
        // The organization context (where the api calls are executed)
        Organization = "69234230193872955",
        // Service account authentication
        ServiceAccountAuthentication = (sa, new()
        {
            ProjectAudiences = { ZitadelDefaults.ZitadelApiProjectId },
        }),
    });

var roles = await api.SearchProjectRolesAsync(
    new() { ProjectId = "84856448403694484" });

foreach (var r in roles.Result)
{
    Console.WriteLine($"{r.Key} : {r.DisplayName} : {r.Group}");
}
```

To see which methods are available, head over to [the api documentation](../zitadel-api/index.md).
