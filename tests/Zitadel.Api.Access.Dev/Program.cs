using System;
using Zitadel.Api;
using Zitadel.Authentication;
using Zitadel.Authentication.Credentials;

Console.WriteLine("Login with Zitadel Service Account");
var sa = await ServiceAccount.LoadFromJsonFileAsync("./service-account.json");
var api = Clients.ManagementService(
    new()
    {
        Endpoint = ZitadelDefaults.ZitadelApiEndpoint,
        Organization = "69234230193872955",
        ServiceAccountAuthentication = (sa, new()
        {
            ProjectAudiences = { ZitadelDefaults.ZitadelApiProjectId },
        }),
    });
Console.WriteLine("Fetch Roles from Zitadel API");
var roles = await api.SearchProjectRolesAsync(
    new() { ProjectId = "84856448403694484" });
Console.WriteLine("Roles:");
foreach (var r in roles.Result)
{
    Console.WriteLine($"{r.Key} : {r.DisplayName} : {r.Group}");
}
