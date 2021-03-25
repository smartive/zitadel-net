using System;
using Zitadel.Api;
using Zitadel.Authentication;
using Zitadel.Authentication.Credentials;

Console.WriteLine("Login with Zitadel Service Account");
var sa = await ServiceAccount.LoadFromJsonFileAsync("./service-account.json");
var token = await sa.AuthenticateAsync(ZitadelDefaults.Issuer);
var api = Clients.ManagementService(
    new()
    {
        Endpoint = "https://api.zitadel.ch",
        Token = token,
    });
var tubel = await api.SearchProjectRolesAsync(new() { ProjectId = "84856448403694484" });
