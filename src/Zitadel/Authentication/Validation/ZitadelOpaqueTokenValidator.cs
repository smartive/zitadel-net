using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Zitadel.Authentication.Validation
{
    public class ZitadelOpaqueTokenValidator : ISecurityTokenValidator
    {
        private const string AuthorizationHeader = "Authorization";

        private static readonly HttpClient Client = new();

        private readonly ConfigurationManager<OpenIdConnectConfiguration> _configuration;

        private readonly string? _primaryDomain;
        private string? _userInfoEndpoint;
        private string? _issuer;

        public ZitadelOpaqueTokenValidator(string discoveryEndpoint, string? primaryDomain)
        {
            _primaryDomain = primaryDomain;
            _configuration = new ConfigurationManager<OpenIdConnectConfiguration>(
                discoveryEndpoint,
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever(Client));
        }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; }

        public bool CanReadToken(string securityToken) => true;

        public ClaimsPrincipal ValidateToken(
            string securityToken,
            TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            validatedToken = new JwtSecurityToken();

            if (_userInfoEndpoint == null || _issuer == null)
            {
                var config = _configuration.GetConfigurationAsync().Result;
                _userInfoEndpoint = config.UserInfoEndpoint;
                _issuer = config.Issuer;
            }

            var response = Client.Send(
                new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(_userInfoEndpoint),
                    Headers = { { AuthorizationHeader, $"Bearer {securityToken}" } },
                });

            var userInfo = response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<UserInfo>()
                .Result;

            if (_primaryDomain != null && userInfo.PrimaryDomain != _primaryDomain)
            {
                // The user-info does not contain a primary domain claim
                // or it was the wrong value.
                return new ClaimsPrincipal();
            }

            var identity = new ClaimsIdentity(
                new[]
                    {
                        Claim(ClaimTypes.NameIdentifier, userInfo.Id),
                        Claim("sub", userInfo.Id),
                        Claim(ZitadelDefaults.PrimaryDomainClaimName, userInfo.PrimaryDomain),

                        Claim(ClaimTypes.Name, userInfo.Name),
                        Claim(ClaimTypes.GivenName, userInfo.GivenName),
                        Claim(ClaimTypes.Surname, userInfo.FamilyName),
                        Claim("nickname", userInfo.Nickname),
                        Claim("preferred_username", userInfo.PreferredUsername),

                        Claim("gender", userInfo.Gender),

                        Claim(ClaimTypes.Email, userInfo.Email),
                        Claim("email_verified", userInfo.EmailVerified.ToString(), ClaimValueTypes.Boolean),

                        Claim(ClaimTypes.Locality, userInfo.Locale),
                        Claim("locale", userInfo.Locale),
                    }
                    .Concat(
                        userInfo.Roles?.Keys.Select(
                            role => Claim(
                                ClaimTypes.Role,
                                role)) ??
                        new Claim[0])
                    .Where(c => c != null)
                    .OfType<Claim>(),
                "Zitadel.OpaqueAccessToken");

            return new ClaimsPrincipal(identity);
        }

        private Claim? Claim(string type, string? value, string valueType = ClaimValueTypes.String)
            => value == null
                ? null
                : new(type, value, valueType, _issuer);

        private readonly struct UserInfo
        {
            [JsonPropertyName("sub")]
            public string Id { get; init; }

            [JsonPropertyName("name")]
            public string Name { get; init; }

            [JsonPropertyName("given_name")]
            public string GivenName { get; init; }

            [JsonPropertyName("family_name")]
            public string FamilyName { get; init; }

            [JsonPropertyName("nickname")]
            public string? Nickname { get; init; }

            [JsonPropertyName("gender")]
            public string? Gender { get; init; }

            [JsonPropertyName("preferred_username")]
            public string PreferredUsername { get; init; }

            [JsonPropertyName("email")]
            public string Email { get; init; }

            [JsonPropertyName("email_verified")]
            public bool EmailVerified { get; init; }

            [JsonPropertyName("locale")]
            public string? Locale { get; init; }

            [JsonPropertyName(ZitadelDefaults.RoleClaimName)]
            public Dictionary<string, Dictionary<string, string>>? Roles { get; init; }

            [JsonPropertyName(ZitadelDefaults.PrimaryDomainClaimName)]
            public string? PrimaryDomain { get; init; }
        }
    }
}
