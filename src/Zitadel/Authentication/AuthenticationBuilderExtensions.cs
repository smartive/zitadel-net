using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Zitadel.Authentication.Handler;
using Zitadel.Authentication.Options;
using Zitadel.Authentication.Validation;

namespace Zitadel.Authentication
{
    /// <summary>
    /// Extensions for the <see cref="AuthenticationBuilder"/>
    /// to add Zitadel login capabilities.
    /// </summary>
    public static class AuthenticationBuilderExtensions
    {
        /// <summary>
        /// Add Zitadel authentication/authorization (via OpenIDConnect) to the application.
        /// This is commonly used when the application delivers server-side
        /// pages. This behaves like other external IDPs (e.g. AddGoogle, AddFacebook, ...).
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
        /// <param name="configureOptions">
        /// An optional action to configure the oidc options
        /// (<see cref="OpenIdConnectOptions"/>).
        /// </param>
        /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddZitadel(
            this AuthenticationBuilder builder,
            Action<OpenIdConnectOptions>? configureOptions = default)
            => builder.AddZitadel(ZitadelDefaults.AuthenticationScheme, configureOptions);

        /// <summary>
        /// Add Zitadel authentication/authorization (via OpenIDConnect) to the application.
        /// This is commonly used when the application delivers server-side
        /// pages. This behaves like other external IDPs (e.g. AddGoogle, AddFacebook, ...).
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
        /// <param name="authenticationScheme">The name for the authentication scheme.</param>
        /// <param name="configureOptions">
        /// An optional action to configure the oidc options
        /// (<see cref="OpenIdConnectOptions"/>).
        /// </param>
        /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddZitadel(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            Action<OpenIdConnectOptions>? configureOptions = default)
            => builder.AddZitadel(authenticationScheme, ZitadelDefaults.DisplayName, configureOptions);

        /// <summary>
        /// Add Zitadel authentication/authorization (via OpenIDConnect) to the application.
        /// This is commonly used when the application delivers server-side
        /// pages. This behaves like other external IDPs (e.g. AddGoogle, AddFacebook, ...).
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
        /// <param name="authenticationScheme">The name for the authentication scheme.</param>
        /// <param name="displayName">The display name for the authentication scheme.</param>
        /// <param name="configureOptions">
        /// An optional action to configure the oidc options
        /// (<see cref="OpenIdConnectOptions"/>).
        /// </param>
        /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
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

        /// <summary>
        /// Add Zitadel authentication/authorization (via OpenIDConnect) to the application.
        /// In contrast to AddZitadel(...), this also adds and configures the external session
        /// cookie. The external cookie is configured to a default with local https since
        /// the same-site restrictions of the Chromium project apply.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
        /// <param name="configureOptions">
        /// An optional action to configure the oidc options
        /// (<see cref="OpenIdConnectOptions"/>).
        /// </param>
        /// <param name="configureCookieOptions">
        /// An optional action to configure the cookie options
        /// (<see cref="CookieAuthenticationOptions"/>).
        /// </param>
        /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddZitadelWithSession(
            this AuthenticationBuilder builder,
            Action<OpenIdConnectOptions>? configureOptions = default,
            Action<CookieAuthenticationOptions>? configureCookieOptions = default)
            => builder.AddZitadelWithSession(
                ZitadelDefaults.AuthenticationScheme,
                configureOptions,
                configureCookieOptions);

        /// <summary>
        /// Add Zitadel authentication/authorization (via OpenIDConnect) to the application.
        /// In contrast to AddZitadel(...), this also adds and configures the external session
        /// cookie. The external cookie is configured to a default with local https since
        /// the same-site restrictions of the Chromium project apply.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
        /// <param name="authenticationScheme">The name for the authentication scheme.</param>
        /// <param name="configureOptions">
        /// An optional action to configure the oidc options
        /// (<see cref="OpenIdConnectOptions"/>).
        /// </param>
        /// <param name="configureCookieOptions">
        /// An optional action to configure the cookie options
        /// (<see cref="CookieAuthenticationOptions"/>).
        /// </param>
        /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddZitadelWithSession(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            Action<OpenIdConnectOptions>? configureOptions = default,
            Action<CookieAuthenticationOptions>? configureCookieOptions = default)
            => builder.AddZitadelWithSession(
                authenticationScheme,
                ZitadelDefaults.DisplayName,
                configureOptions,
                configureCookieOptions);

        /// <summary>
        /// Add Zitadel authentication/authorization (via OpenIDConnect) to the application.
        /// In contrast to AddZitadel(...), this also adds and configures the external session
        /// cookie. The external cookie is configured to a default with local https since
        /// the same-site restrictions of the Chromium project apply.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
        /// <param name="authenticationScheme">The name for the authentication scheme.</param>
        /// <param name="displayName">The display name for the authentication scheme.</param>
        /// <param name="configureOptions">
        /// An optional action to configure the oidc options
        /// (<see cref="OpenIdConnectOptions"/>).
        /// </param>
        /// <param name="configureCookieOptions">
        /// An optional action to configure the cookie options
        /// (<see cref="CookieAuthenticationOptions"/>).
        /// </param>
        /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddZitadelWithSession(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            string displayName,
            Action<OpenIdConnectOptions>? configureOptions = default,
            Action<CookieAuthenticationOptions>? configureCookieOptions = default)
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
                        configureCookieOptions?.Invoke(options);
                    })
                .Services;

        /// <summary>
        /// Add the Zitadel authentication handler without caring for session handling.
        /// This is typically used by Single Page Applications (SPA) that handle
        /// the login flow and just send the received JWT or opaque token to an api.
        /// This handler can manage JWT as well as opaque access tokens.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
        /// <param name="configureOptions">
        /// An optional action to configure the zitadel handler options
        /// (<see cref="ZitadelHandlerOptions"/>).
        /// </param>
        /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddZitadelAuthenticationHandler(
            this AuthenticationBuilder builder,
            Action<ZitadelHandlerOptions>? configureOptions = default)
            => builder.AddZitadelAuthenticationHandler(ZitadelDefaults.HandlerAuthenticationScheme, configureOptions);

        /// <summary>
        /// Add the Zitadel authentication handler without caring for session handling.
        /// This is typically used by Single Page Applications (SPA) that handle
        /// the login flow and just send the received JWT or opaque token to an api.
        /// This handler can manage JWT as well as opaque access tokens.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
        /// <param name="authenticationScheme">The name for the authentication scheme.</param>
        /// <param name="configureOptions">
        /// An optional action to configure the zitadel handler options
        /// (<see cref="ZitadelHandlerOptions"/>).
        /// </param>
        /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddZitadelAuthenticationHandler(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            Action<ZitadelHandlerOptions>? configureOptions = default)
            => builder.AddZitadelAuthenticationHandler(
                authenticationScheme,
                ZitadelDefaults.DisplayName,
                configureOptions);

        /// <summary>
        /// Add the Zitadel authentication handler without caring for session handling.
        /// This is typically used by Single Page Applications (SPA) that handle
        /// the login flow and just send the received JWT or opaque token to an api.
        /// This handler can manage JWT as well as opaque access tokens.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
        /// <param name="authenticationScheme">The name for the authentication scheme.</param>
        /// <param name="displayName">The display name for the authentication scheme.</param>
        /// <param name="configureOptions">
        /// An optional action to configure the zitadel handler options
        /// (<see cref="ZitadelHandlerOptions"/>).
        /// </param>
        /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
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
                        };

                        options.SecurityTokenValidators.Clear();
                        options.SecurityTokenValidators.Add(new ZitadelJwtTokenValidator(zitadelOptions.PrimaryDomain));
                        options.SecurityTokenValidators.Add(
                            new ZitadelOpaqueTokenValidator(
                                zitadelOptions.DiscoveryEndpoint,
                                zitadelOptions.PrimaryDomain));
                    });

        /// <summary>
        /// Add a "fake" zitadel authentication. This should only be used for local
        /// development to fake an authentication/authorization. All calls are authenticated
        /// by default. If (e.g. for testing reasons) a specific call should NOT be authenticated,
        /// attach the header "x-zitadel-fake-auth" with the value "false" to the request.
        /// This specific request will then fail to authenticate.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
        /// <param name="configureOptions">Action to configure the <see cref="LocalFakeZitadelOptions"/>.</param>
        /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddFakeZitadel(
            this AuthenticationBuilder builder,
            Action<LocalFakeZitadelOptions>? configureOptions)
        {
            var options = new LocalFakeZitadelOptions();
            configureOptions?.Invoke(options);
            return builder.AddFakeZitadel(options);
        }

        /// <summary>
        /// Add a "fake" zitadel authentication. This should only be used for local
        /// development to fake an authentication/authorization. All calls are authenticated
        /// by default. If (e.g. for testing reasons) a specific call should NOT be authenticated,
        /// attach the header "x-zitadel-fake-auth" with the value "false" to the request.
        /// This specific request will then fail to authenticate.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
        /// <param name="options">The <see cref="LocalFakeZitadelOptions"/> to use.</param>
        /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddFakeZitadel(
            this AuthenticationBuilder builder,
            LocalFakeZitadelOptions options)
            => builder.AddScheme<LocalFakeZitadelSchemeOptions, LocalFakeZitadelHandler>(
                ZitadelDefaults.FakeAuthenticationScheme,
                o => o.FakeZitadelOptions = options);
    }
}
