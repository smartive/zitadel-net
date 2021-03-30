using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Zitadel.Authentication.Credentials;
using Zitadel.Authentication.Models;
using Zitadel.Authentication.Options;

namespace Zitadel.Authentication.Validation
{
    internal class ZitadelApiValidator : JwtSecurityTokenHandler, IDisposable
    {
        private const string AuthorizationHeader = "Authorization";
        private static readonly HttpClient Client = new();

        private readonly ZitadelApiOptions _options;

        private readonly Func<string, HttpRequestMessage> _requestConstructor;
        private readonly ConfigurationManager<OpenIdConnectConfiguration> _configuration;
        private OpenIdConnectConfiguration? _oidcConfiguration;
        private IDisposable? _appRenew;

        public ZitadelApiValidator(ZitadelApiOptions options)
        {
            _options = options;
            _configuration = new(
                options.DiscoveryEndpoint,
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever(Client));
            _requestConstructor = RequestConstructor();
        }

        public void Dispose()
        {
            _appRenew?.Dispose();
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
                    .Concat(introspection.CreateRoleClaims(_options.Issuer))
                    .Where(c => c != null)
                    .OfType<Claim>(),
                $"Zitadel.{(isJwtToken ? "JwtToken" : "OpaqueToken")}");

            return new(identity);
        }

        private Func<string, HttpRequestMessage> RequestConstructor()
        {
            _oidcConfiguration ??= _configuration.GetConfigurationAsync().Result;
            if (_options.BasicAuthCredentials == null && _options.JwtProfileKey == null)
            {
                throw new ApplicationException(
                    "Neither BasicAuth nor JwtPrivateKey credentials configured in Zitadel API authentication.");
            }

            if (_options.JwtProfileKey != null)
            {
                var app = _options.JwtProfileKey.Content != null
                    ? Application.LoadFromJsonString(_options.JwtProfileKey.Content)
                    : Application.LoadFromJsonFile(_options.JwtProfileKey.Path ?? string.Empty);

                string? jwt = null;

                _appRenew = Observable
                    .Timer(TimeSpan.Zero, TimeSpan.FromMinutes(55))
                    .Select(_ => app.GetSignedJwt(_options.Issuer, TimeSpan.FromHours(1)))
                    .Subscribe(appJwt => jwt = appJwt);

                return token =>
                {
                    jwt ??= app.GetSignedJwt(_options.Issuer);

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

            return token => new()
            {
                Method = HttpMethod.Post,
                RequestUri = new(_oidcConfiguration.IntrospectionEndpoint),
                Headers = { { AuthorizationHeader, _options.BasicAuthCredentials!.HttpCredentials } },
                Content = new FormUrlEncodedContent(new[] { new KeyValuePair<string?, string?>("token", token) }),
            };
        }

        private Claim? Claim(string type, string? value, string valueType = ClaimValueTypes.String)
            => value == null
                ? null
                : new(type, value, valueType, _options.Issuer);
    }
}
