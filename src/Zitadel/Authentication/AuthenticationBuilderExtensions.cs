using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Zitadel.Authentication.Options;
using Zitadel.Authentication.Validation;

namespace Zitadel.Authentication
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddZitadel(
            this AuthenticationBuilder builder,
            Action<OpenIdConnectOptions>? configureOptions = default)
            => builder.AddZitadel(ZitadelDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddZitadel(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            Action<OpenIdConnectOptions>? configureOptions = default)
            => builder.AddZitadel(authenticationScheme, ZitadelDefaults.DisplayName, configureOptions);

        public static AuthenticationBuilder AddZitadel(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            string displayName,
            Action<OpenIdConnectOptions>? configureOptions = default) =>
            builder.AddOpenIdConnect(
                authenticationScheme,
                displayName,
                options =>
                {
                    options.Authority = ZitadelDefaults.Issuer;
                    options.CallbackPath = ZitadelDefaults.CallbackPath;
                    options.UsePkce = true;
                    options.ResponseType = "code";
                    options.ClaimActions.Add(new ZitadelProjectRolesClaimAction());

                    configureOptions?.Invoke(options);
                });

        public static IServiceCollection AddZitadelWithSession(
            this AuthenticationBuilder builder,
            Action<OpenIdConnectOptions>? configureOptions = default)
            => builder.AddZitadelWithSession(ZitadelDefaults.AuthenticationScheme, configureOptions);

        public static IServiceCollection AddZitadelWithSession(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            Action<OpenIdConnectOptions>? configureOptions = default)
            => builder.AddZitadelWithSession(authenticationScheme, ZitadelDefaults.DisplayName, configureOptions);

        public static IServiceCollection AddZitadelWithSession(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            string displayName,
            Action<OpenIdConnectOptions>? configureOptions = default)
            => builder
                .AddZitadel(
                    authenticationScheme,
                    displayName,
                    options =>
                    {
                        options.SignInScheme = IdentityConstants.ExternalScheme;
                        configureOptions?.Invoke(options);
                    })
                .AddExternalCookie()
                .Configure(
                    options =>
                    {
                        options.Cookie.HttpOnly = true;
                        options.Cookie.IsEssential = true;
                        options.Cookie.SameSite = SameSiteMode.None;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    })
                .Services;

        public static AuthenticationBuilder AddZitadelAuthenticationHandler(
            this AuthenticationBuilder builder,
            Action<ZitadelHandlerOptions>? configureOptions = default)
            => builder.AddZitadelAuthenticationHandler(ZitadelDefaults.HandlerAuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddZitadelAuthenticationHandler(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            Action<ZitadelHandlerOptions>? configureOptions = default)
            => builder.AddZitadelAuthenticationHandler(
                authenticationScheme,
                ZitadelDefaults.DisplayName,
                configureOptions);

        public static AuthenticationBuilder AddZitadelAuthenticationHandler(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            string displayName,
            Action<ZitadelHandlerOptions>? configureOptions = default)
            => builder
                .AddJwtBearer(
                    authenticationScheme,
                    displayName,
                    options =>
                    {
                        var zitadelOptions = new ZitadelHandlerOptions();
                        configureOptions?.Invoke(zitadelOptions);

                        options.Authority = zitadelOptions.Authority;
                        options.Audience = zitadelOptions.ClientId;

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            ValidateAudience = zitadelOptions.ValidateAudience,
                            ValidAudiences = zitadelOptions.ValidAudiences ?? new[] { zitadelOptions.ClientId },
                            ValidIssuer = zitadelOptions.Issuer,
                            PropertyBag = new Dictionary<string, object>(),
                        };

                        if (!string.IsNullOrWhiteSpace(zitadelOptions.PrimaryDomain))
                        {
                            options.TokenValidationParameters.PropertyBag.Add(
                                "primaryDomain",
                                zitadelOptions.PrimaryDomain);
                        }

                        options.SecurityTokenValidators.Clear();
                        options.SecurityTokenValidators.Add(new ZitadelJwtTokenValidator(zitadelOptions.PrimaryDomain));
                        options.SecurityTokenValidators.Add(
                            new ZitadelOpaqueTokenValidator(
                                zitadelOptions.DiscoveryEndpoint,
                                zitadelOptions.PrimaryDomain));
                    });
    }
}
