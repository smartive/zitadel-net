using Zitadel.Credentials;
using Zitadel.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services
    .AddAuthorization()
    .AddAuthentication()
    .AddZitadelIntrospection(
        "ZITADEL_JWT",
        o =>
        {
            o.Authority = "https://zitadel-libraries-l8boqa.zitadel.cloud";
            o.JwtProfile = Application.LoadFromJsonString(
                @"
{
  ""type"": ""application"",
  ""keyId"": ""170104948032274689"",
  ""key"": ""-----BEGIN RSA PRIVATE KEY-----\nMIIEpAIBAAKCAQEAq4+TOYQH7p/saqLYUnLJwjtT6sUCkktlkK3qqoLdn0HLmHZW\njNcHF6U2F2/HumW7RFIcmaoxxm0limS4UqNzXpqkNqF75RaSrldU5Phink5ZkTLs\nEciaaoy4eYI5rtuHhWbN6SO70eY2/XZ1TlOAL4DJbbW4p4YhKOz0Naui8r8dBqWw\nwvXdkfqLfzL43HCGES+aIaqcv99RI/wJe0ogQ+7P4enP1SeauXavBRfkEhn/wrNH\nSEbVKQ48dKdlzmNbAocsOprfs2p2muaRHlz9waqJNhdUwYEotiYhK+LeUaP8IVCJ\nN5OU/io+ifuXIXkpag3PzAQ8Fth3Au74WzzHSQIDAQABAoIBAEtoUifntqzWMk40\nwayLs87h0OLSMW0oIr5TE2BbIRqNCvY6nZRON1nXTk1C3qE5cfR3uwZ33mT/OI75\n8mKwYVdl1WQF2rU5FMP4suHpoz895PSDU2wFponKzJLsAHqxF4I1S7B7+mQqMmV6\nGdmRrjgy/VZxl3Za6FxauoSUqozTZ5vNS/1Ig+/Ri7qD4zM5HKbr7JCICuGJXPhM\nWL5CBe9sOSDtTBuzg4bp+XxVFq0mFuKlR2yG6/Ky+pDHNFxDLIrOxmSgkDayLI9r\nmxbrUGTdBiNuFe5Ezl/6WdEhzObVpxFJ4cZcCG4DH+O4F9mDasDx0wKLl+t1t7Bw\nmaur9RkCgYEA12lpRUVhsKVtREWn5I206QuCrNqkfAPJ3CcNf8YMwzsEEaWS/44V\n0oSheoHdVJ2XV56N0sLPKWJO+dqOnU2PIKoM6HeZKjKLke47+EIZaRNTrhhJzPP+\nwHIv6Jz44j2EHWi52FF4o2WqY4NS9OEWkrpapkJajfCtQAUatvUzQBMCgYEAy+L5\nMz+K9SnuKzHMFObpZobMjS/OO87wn+XLx8UItwMaergz7oCgEgSv8nYfyJ1LigHy\nkT748Ag8gLl4WCPpCMGQYfuk5zBDBW+KNU2bDZiyFNMmVd9LcbJQiGdsZIyJ9t4e\nebnZeYyYQp+j7XB4cseMKpVIHT7XqpxITSmgXrMCgYEAnzZ7J0bzwHNUwoxVXnla\niJEIYagswLiwHzcCJDmGv1nEVSKy9o3XFUUQcRLBO0RLUuiO3IM+SNEvnD5tAFkN\n+8+UQNH89BJt1EtoKcL5Mw+k3t121rRUy3rabCxxTA65sl7wVbFJ4ENJX8n1q6ce\nXw6753zNn3GPK+1Z5HZxDd8CgYAAsmXnpu/yppIJ08G+0Is7rnpEgUVTLwHjigWI\nSUQeXARbJwYGaqohZaK0UXMKXH9FmXwawvxW1bBfQEMJChZh0UeNDi8iGygffKIc\nTIebJEp3h8E5yemYGePsk23rag+OqHOyNtBnefOLRsBor1m6CrSP8LKuZuiVzLLy\nkJHbwQKBgQC9/r6I5woj+Khm2izGRAyPOhTkDbpFWr2IU5AZ3jUin5yZxXOARcUM\nv5HIVFHC/X8O4rBFfAoNGjpef99icPbIXc60hbYrCD8Q+Nky8Li7XzlqKweF8nPB\nSo9s/gLF0gZcoYxf9M9bLB5bh93P6qYGk7ybaGbe7P6aUhSHV6pZgw==\n-----END RSA PRIVATE KEY-----\n"",
  ""appId"": ""170101999168127233"",
  ""clientId"": ""170101999168192769@library""
}");
        })
    .AddZitadelIntrospection(
        "ZITADEL_BASIC",
        o =>
        {
            o.Authority = "https://zitadel-libraries-l8boqa.zitadel.cloud/";
            o.ClientId = "170102032621961473@library";
            o.ClientSecret = "KNkKW8nx3rlEKOeHNUcPx80tZTP1uZTjJESfdA3kMEK7urhX3ChFukTMQrtjvG70";
        })
    .AddZitadelFake("ZITADEL_FAKE",
        o =>
        {
            o.FakeZitadelId = "1337";
        });

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(
    endpoints => { endpoints.MapControllers(); });

await app.RunAsync();
