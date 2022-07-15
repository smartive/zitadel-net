﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OpenIdConnect.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Zitadel.Authentication;
using Zitadel.Authentication.Handler;
using Zitadel.Authentication.Options;

namespace Zitadel.Extensions;

/// <summary>
/// Extensions for the <see cref="AuthenticationBuilder"/>
/// to add ZITADEL login capabilities.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Add ZITADEL authentication/authorization (via OpenIDConnect) to the application.
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
    /// Add ZITADEL authentication/authorization (via OpenIDConnect) to the application.
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
    /// Add ZITADEL authentication/authorization (via OpenIDConnect) to the application.
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
                options.CallbackPath = ZitadelDefaults.CallbackPath;
                options.UsePkce = true;
                options.ResponseType = "code";
                options.GetClaimsFromUserInfoEndpoint = true;

                options.ClaimActions.Add(
                    new UniqueJsonKeyClaimAction(ClaimTypes.NameIdentifier, ClaimValueTypes.String, "sub"));
                options.ClaimActions.Add(
                    new JsonKeyClaimAction(
                        ZitadelDefaults.PrimaryDomainClaimName,
                        ClaimValueTypes.String,
                        ZitadelDefaults.PrimaryDomainClaimName));
                options.ClaimActions.Add(
                    new JsonKeyClaimAction(ClaimTypes.Name, ClaimValueTypes.String, "name"));
                options.ClaimActions.Add(
                    new JsonKeyClaimAction(ClaimTypes.GivenName, ClaimValueTypes.String, "given_name"));
                options.ClaimActions.Add(
                    new JsonKeyClaimAction(ClaimTypes.Surname, ClaimValueTypes.String, "family_name"));
                options.ClaimActions.Add(
                    new JsonKeyClaimAction("nickname", ClaimValueTypes.String, "nickname"));
                options.ClaimActions.Add(
                    new JsonKeyClaimAction("preferred_username", ClaimValueTypes.String, "preferred_username"));
                options.ClaimActions.Add(
                    new JsonKeyClaimAction("gender", ClaimValueTypes.String, "gender"));
                options.ClaimActions.Add(
                    new JsonKeyClaimAction(ClaimTypes.Email, ClaimValueTypes.String, "email"));
                options.ClaimActions.Add(
                    new JsonKeyClaimAction("email_verified", ClaimValueTypes.Boolean, "email_verified"));
                options.ClaimActions.Add(
                    new JsonKeyClaimAction(ClaimTypes.Locality, ClaimValueTypes.String, "locale"));
                options.ClaimActions.Add(
                    new JsonKeyClaimAction("locale", ClaimValueTypes.String, "locale"));
                options.ClaimActions.Add(new ZitadelProjectRolesClaimAction());
                options.ClaimActions.Add(new DeleteClaimAction(ZitadelDefaults.RoleClaimName));

                configureOptions?.Invoke(options);
            });

    // /// <summary>
    // /// Add the ZITADEL introspection handler without caring for session handling.
    // /// This is typically used by Single Page Applications (SPA) that handle
    // /// the login flow and just send the received JWT or opaque token to an api.
    // /// This handler can manage JWT as well as opaque access tokens.
    // /// </summary>
    // /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
    // /// <param name="configureOptions">
    // /// An optional action to configure the ZITADEL handler options
    // /// (<see cref="ZitadelIntrospectionOptions"/>).
    // /// </param>
    // /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
    // public static AuthenticationBuilder AddZitadelIntrospection(
    //     this AuthenticationBuilder builder,
    //     Action<ZitadelIntrospectionOptions>? configureOptions = default)
    //     => builder.AddZitadelIntrospection(ZitadelDefaults.AuthenticationScheme, configureOptions);
    
    // public static AuthenticationBuilder AddZitadelIntrospection(
    //     this AuthenticationBuilder builder,
    //     string authenticationScheme,
    //     string displayName,
    //     Action<ZitadelIntrospectionOptions>? configureOptions = default) =>
    //     builder
    //         .AddJwtBearer(
    //             authenticationScheme,
    //             displayName,
    //             options =>
    //             {
    //                 var zitadelOptions = new ZitadelIntrospectionOptions();
    //                 configureOptions?.Invoke(zitadelOptions);
    //
    //                 options.Authority = zitadelOptions.Authority;
    //                 options.Audience = zitadelOptions.ClientId;
    //
    //                 options.TokenValidationParameters = new TokenValidationParameters
    //                 {
    //                     ValidateIssuerSigningKey = true,
    //                     ValidateAudience = zitadelOptions.ValidateAudience,
    //                     ValidAudiences = zitadelOptions.ValidAudiences?
    //                                          .Append(zitadelOptions.ClientId)
    //                                          .Distinct() ??
    //                                      new[] { zitadelOptions.ClientId },
    //                     ValidIssuer = zitadelOptions.Authority,
    //                 };
    //
    //                 options.SecurityTokenValidators.Clear();
    //                 options.SecurityTokenValidators.Add(new ZitadelIntrospectionHandler(zitadelOptions));
    //             });
    
    // public static AuthenticationBuilder AddZitadelIntrospection(
    //     this AuthenticationBuilder builder,
    //     string authenticationScheme,
    //     Action<ZitadelIntrospectionOptions>? configureOptions = default) =>
    //     builder
    //         .AddOAuth2Introspection(
    //             authenticationScheme,
    //             options =>
    //             {
    //                 var zitadelOptions = new ZitadelIntrospectionOptions
    //                 {
    //                     ClientCredentialStyle = ClientCredentialStyle.AuthorizationHeader,
    //                     AuthorizationHeaderStyle = BasicAuthenticationHeaderStyle.Rfc6749,
    //                 };
    //                 configureOptions?.Invoke(zitadelOptions);
    //
    //                 // copy all properties from zitadeloptions to options
    //                 options.Authority = zitadelOptions.Authority;
    //                 options.Events = zitadelOptions.Events;
    //                 options.AuthenticationType = zitadelOptions.AuthenticationType;
    //                 options.CacheDuration = zitadelOptions.CacheDuration;
    //                 options.ClientId = zitadelOptions.ClientId;
    //                 options.ClientSecret = zitadelOptions.ClientSecret;
    //                 options.DiscoveryPolicy = zitadelOptions.DiscoveryPolicy;
    //                 options.EnableCaching = zitadelOptions.EnableCaching;
    //                 options.IntrospectionEndpoint = zitadelOptions.IntrospectionEndpoint;
    //                 options.SaveToken = zitadelOptions.SaveToken;
    //                 options.TokenRetriever = zitadelOptions.TokenRetriever;
    //                 options.AuthorizationHeaderStyle = zitadelOptions.AuthorizationHeaderStyle;
    //                 options.CacheKeyGenerator = zitadelOptions.CacheKeyGenerator;
    //                 options.CacheKeyPrefix = zitadelOptions.CacheKeyPrefix;
    //                 options.ClientCredentialStyle = zitadelOptions.ClientCredentialStyle;
    //                 options.NameClaimType = zitadelOptions.NameClaimType;
    //                 options.RoleClaimType = zitadelOptions.RoleClaimType;
    //                 options.TokenTypeHint = zitadelOptions.TokenTypeHint;
    //                 options.SkipTokensWithDots = zitadelOptions.SkipTokensWithDots;
    //                 options.ClaimsIssuer = zitadelOptions.ClaimsIssuer;
    //                 options.EventsType = zitadelOptions.EventsType;
    //                 options.ForwardAuthenticate = zitadelOptions.ForwardAuthenticate;
    //                 options.ForwardChallenge = zitadelOptions.ForwardChallenge;
    //                 options.ForwardDefault = zitadelOptions.ForwardDefault;
    //                 options.ForwardForbid = zitadelOptions.ForwardForbid;
    //                 options.ForwardDefaultSelector = zitadelOptions.ForwardDefaultSelector;
    //                 options.ForwardSignIn = zitadelOptions.ForwardSignIn;
    //                 options.ForwardSignOut = zitadelOptions.ForwardSignOut;
    //
    //                 if (zitadelOptions.JwtProfile == null)
    //                 {
    //                     return;
    //                 }
    //
    //                 options.ClientCredentialStyle = ClientCredentialStyle.PostBody;
    //                 options.ClientSecret = null;
    //                 options.TokenTypeHint = null;
    //                 // options.Events.OnSendingRequest += async context =>
    //                 // {
    //                 //     var c = context;
    //                 //     c.TokenIntrospectionRequest.ClientId = null;
    //                 // };
    //                 options.Events.OnUpdateClientAssertion += async context =>
    //                 {
    //                     var jwt = await zitadelOptions.JwtProfile.GetSignedJwtAsync(options.Authority);
    //                     context.ClientAssertion = new()
    //                     {
    //                         Type = ZitadelIntrospectionOptions.JwtBearerClientAssertionType,
    //                         Value = jwt,
    //                     };
    //                     context.ClientAssertionExpirationTime = DateTime.UtcNow.AddMinutes(55);
    //                 };
    //             });

    /// <summary>
    /// Add a "fake" ZITADEL authentication. This should only be used for local
    /// development to fake an authentication/authorization. All calls are authenticated
    /// by default. If (e.g. for testing reasons) a specific call should NOT be authenticated,
    /// attach the header "x-zitadel-fake-auth" with the value "false" to the request.
    /// This specific request will then fail to authenticate.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
    /// <param name="configureOptions">Action to configure the <see cref="LocalFakeZitadelOptions"/>.</param>
    /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddZitadelMock(
        this AuthenticationBuilder builder,
        Action<LocalFakeZitadelOptions>? configureOptions)
        => builder.AddZitadelMock(
            ZitadelDefaults.MockAuthenticationScheme,
            configureOptions);

    /// <summary>
    /// Add a "fake" ZITADEL authentication. This should only be used for local
    /// development to fake an authentication/authorization. All calls are authenticated
    /// by default. If (e.g. for testing reasons) a specific call should NOT be authenticated,
    /// attach the header "x-zitadel-fake-auth" with the value "false" to the request.
    /// This specific request will then fail to authenticate.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
    /// <param name="authenticationScheme">The name for the authentication scheme to be used.</param>
    /// <param name="configureOptions">Action to configure the <see cref="LocalFakeZitadelOptions"/>.</param>
    /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddZitadelMock(
        this AuthenticationBuilder builder,
        string authenticationScheme,
        Action<LocalFakeZitadelOptions>? configureOptions)
        => builder.AddZitadelMock(authenticationScheme, ZitadelDefaults.FakeDisplayName, configureOptions);

    /// <summary>
    /// Add a "fake" ZITADEL authentication. This should only be used for local
    /// development to fake an authentication/authorization. All calls are authenticated
    /// by default. If (e.g. for testing reasons) a specific call should NOT be authenticated,
    /// attach the header "x-zitadel-fake-auth" with the value "false" to the request.
    /// This specific request will then fail to authenticate.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
    /// <param name="authenticationScheme">The name for the authentication scheme to be used.</param>
    /// <param name="displayName">The display name for the authentication scheme.</param>
    /// <param name="configureOptions">Action to configure the <see cref="LocalFakeZitadelOptions"/>.</param>
    /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddZitadelMock(
        this AuthenticationBuilder builder,
        string authenticationScheme,
        string displayName,
        Action<LocalFakeZitadelOptions>? configureOptions)
    {
        var options = new LocalFakeZitadelOptions();
        configureOptions?.Invoke(options);
        return builder.AddZitadelMock(authenticationScheme, displayName, options);
    }

    /// <summary>
    /// Add a "fake" ZITADEL authentication. This should only be used for local
    /// development to fake an authentication/authorization. All calls are authenticated
    /// by default. If (e.g. for testing reasons) a specific call should NOT be authenticated,
    /// attach the header "x-zitadel-fake-auth" with the value "false" to the request.
    /// This specific request will then fail to authenticate.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
    /// <param name="options">The <see cref="LocalFakeZitadelOptions"/> to use.</param>
    /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddZitadelMock(
        this AuthenticationBuilder builder,
        LocalFakeZitadelOptions options)
        => builder.AddZitadelMock(
            ZitadelDefaults.MockAuthenticationScheme,
            options);

    /// <summary>
    /// Add a "fake" ZITADEL authentication. This should only be used for local
    /// development to fake an authentication/authorization. All calls are authenticated
    /// by default. If (e.g. for testing reasons) a specific call should NOT be authenticated,
    /// attach the header "x-zitadel-fake-auth" with the value "false" to the request.
    /// This specific request will then fail to authenticate.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
    /// <param name="authenticationScheme">The name for the authentication scheme to be used.</param>
    /// <param name="options">The <see cref="LocalFakeZitadelOptions"/> to use.</param>
    /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddZitadelMock(
        this AuthenticationBuilder builder,
        string authenticationScheme,
        LocalFakeZitadelOptions options)
        => builder.AddZitadelMock(authenticationScheme, ZitadelDefaults.FakeDisplayName, options);

    /// <summary>
    /// Add a "fake" ZITADEL authentication. This should only be used for local
    /// development to fake an authentication/authorization. All calls are authenticated
    /// by default. If (e.g. for testing reasons) a specific call should NOT be authenticated,
    /// attach the header "x-zitadel-fake-auth" with the value "false" to the request.
    /// This specific request will then fail to authenticate.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/> to configure.</param>
    /// <param name="authenticationScheme">The name for the authentication scheme to be used.</param>
    /// <param name="displayName">The display name for the authentication scheme.</param>
    /// <param name="options">The <see cref="LocalFakeZitadelOptions"/> to use.</param>
    /// <returns>The configured <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddZitadelMock(
        this AuthenticationBuilder builder,
        string authenticationScheme,
        string displayName,
        LocalFakeZitadelOptions options)
        => builder.AddScheme<LocalFakeZitadelSchemeOptions, LocalFakeZitadelHandler>(
            authenticationScheme,
            displayName,
            o => o.FakeZitadelOptions = options);
}
