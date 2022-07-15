using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Zitadel.Authentication.Options;
using Zitadel.Credentials;

namespace Zitadel.Authentication.Handler;

internal class ZitadelIntrospectionHandler : JwtSecurityTokenHandler
{
    private const string AuthorizationHeader = "Authorization";
    private static readonly HttpClient Client = new();

    private readonly ZitadelIntrospectionOptions _options;

    private readonly Func<string, HttpRequestMessage> _requestConstructor;
    private readonly ConfigurationManager<OpenIdConnectConfiguration> _configuration;
    private OpenIdConnectConfiguration? _oidcConfiguration;

    public ZitadelIntrospectionHandler(ZitadelIntrospectionOptions options)
    {
        _options = options;
        _configuration = new(
            options.DiscoveryEndpoint ?? DiscoveryEndpoint(options.Authority),
            new OpenIdConnectConfigurationRetriever(),
            new HttpDocumentRetriever(Client));
        _requestConstructor = RequestConstructor();
    }

    public override bool CanReadToken(string token) => true;

    public override ClaimsPrincipal ValidateToken(
        string token,
        TokenValidationParameters validationParameters,
        out SecurityToken validatedToken)
    {
        /*
         * The validation logic of an API resource is done by the following steps:
         * 1. In the case of a JWT token, validate the token first (audience, issuer, signature, ...)
         * 2. Call the introspect endpoint and retrieve the according response document.
         * 3. Parse the ClaimIdentity out of the introspect response.
         */
        var isJwtToken = base.CanReadToken(token);

        if (isJwtToken)
        {
            base.ValidateToken(token, validationParameters, out validatedToken);
        }
        else
        {
            // In case of an opaque bearer token, this is set to an empty token.
            validatedToken = new JwtSecurityToken();
        }

        var response = Client.Send(_requestConstructor(token));

        if (!response.IsSuccessStatusCode)
        {
            // The authentication on the introspect endpoint was not successful.
            return new();
        }

        var introspection = response
            .Content
            .ReadFromJsonAsync<IntrospectResponse>()
            .Result;

        if (introspection == null)
        {
            throw new ApplicationException("Introspection-result could not be parsed.");
        }

        if (!introspection.Active)
        {
            // The token provided by the client is not valid anymore (for any reason).
            return new();
        }

        if (_options.PrimaryDomain != null && introspection.PrimaryDomain != _options.PrimaryDomain)
        {
            // The introspect result does not contain a primary domain claim
            // or it was the wrong value.
            return new();
        }

        var identity = new ClaimsIdentity(
            new[]
                {
                    Claim(ClaimTypes.NameIdentifier, introspection.Id),
                    Claim("sub", introspection.Id),
                    Claim(ZitadelDefaults.PrimaryDomainClaimName, introspection.PrimaryDomain),
                    Claim("client_id", introspection.ClientId),

                    Claim(ClaimTypes.Name, introspection.Name),
                    Claim(ClaimTypes.GivenName, introspection.GivenName),
                    Claim(ClaimTypes.Surname, introspection.FamilyName),
                    Claim("nickname", introspection.Nickname),
                    Claim("preferred_username", introspection.PreferredUsername),

                    Claim("gender", introspection.Gender),

                    Claim(ClaimTypes.Email, introspection.Email),
                    Claim("email_verified", introspection.EmailVerified.ToString(), ClaimValueTypes.Boolean),

                    Claim(ClaimTypes.Locality, introspection.Locale),
                    Claim("locale", introspection.Locale),
                }
                .Concat(introspection.CreateRoleClaims(_options.Authority))
                .Where(c => c != null)
                .OfType<Claim>(),
            $"ZITADEL.{(isJwtToken ? "JwtToken" : "OpaqueToken")}");

        return new(identity);
    }

    private static string DiscoveryEndpoint(string discoveryEndpoint) =>
        discoveryEndpoint.EndsWith(ZitadelDefaults.DiscoveryEndpointPath)
            ? discoveryEndpoint
            : discoveryEndpoint.TrimEnd('/') + ZitadelDefaults.DiscoveryEndpointPath;

    private Func<string, HttpRequestMessage> RequestConstructor()
    {
        _oidcConfiguration ??= _configuration.GetConfigurationAsync().Result;
        if (_options.JwtProfile != null)
        {
            return token =>
            {
                var jwt = _options.JwtProfile?.GetSignedJwt(_options.Authority);

                return new()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new(_oidcConfiguration.IntrospectionEndpoint),
                    Content = new FormUrlEncodedContent(
                        new[]
                        {
                            new KeyValuePair<string?, string?>(
                                "client_assertion_type",
                                "urn:ietf:params:oauth:client-assertion-type:jwt-bearer"),
                            new KeyValuePair<string?, string?>(
                                "client_assertion",
                                $"{jwt}"),
                            new KeyValuePair<string?, string?>("token", token),
                        }),
                };
            };
        }

        var credentials = new BasicAuthentication(_options.ClientId, _options.ClientSecret ?? string.Empty);
        return token => new()
        {
            Method = HttpMethod.Post,
            RequestUri = new(_oidcConfiguration.IntrospectionEndpoint),
            Headers = { { AuthorizationHeader, credentials.HttpCredentials } },
            Content = new FormUrlEncodedContent(new[] { new KeyValuePair<string?, string?>("token", token) }),
        };
    }

    private Claim? Claim(string type, string? value, string valueType = ClaimValueTypes.String)
        => value == null
            ? null
            : new(type, value, valueType, _options.Authority);

    private record IntrospectResponse
    {
        [JsonPropertyName("active")]
        public bool Active { get; init; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; init; } = string.Empty;

        [JsonPropertyName("sub")]
        public string Id { get; init; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("given_name")]
        public string GivenName { get; init; } = string.Empty;

        [JsonPropertyName("family_name")]
        public string FamilyName { get; init; } = string.Empty;

        [JsonPropertyName("nickname")]
        public string? Nickname { get; init; }

        [JsonPropertyName("gender")]
        public string? Gender { get; init; }

        [JsonPropertyName("username")]
        public string PreferredUsername { get; init; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; init; } = string.Empty;

        [JsonPropertyName("email_verified")]
        public bool EmailVerified { get; init; }

        [JsonPropertyName("locale")]
        public string? Locale { get; init; }

        [JsonPropertyName(ZitadelDefaults.RoleClaimName)]
        public Dictionary<string, Dictionary<string, string>>? Roles { get; init; }

        /// <summary>
        /// The primary domain (if requested).
        /// </summary>
        [JsonPropertyName(ZitadelDefaults.PrimaryDomainClaimName)]
        public string? PrimaryDomain { get; init; }

        public IEnumerable<Claim> CreateRoleClaims(string issuer)
        {
            if (Roles == null || Roles.Count == 0)
            {
                return Array.Empty<Claim>();
            }

            // First create the flat list of all possible roles
            // then attach organization specific roles.
            return Roles.SelectMany(
                role => role.Value
                    .Select(
                        org => new Claim(
                            ZitadelDefaults.OrganizationRoleClaimName(org.Key),
                            role.Key,
                            ClaimValueTypes.String,
                            issuer))
                    .Append(new(ClaimTypes.Role, role.Key, ClaimValueTypes.String, issuer)));
        }
    }
}
