using IdentityModel.AspNetCore.OAuth2Introspection;

using Zitadel.Credentials;

namespace Zitadel.Authentication.Options;

/// <summary>
/// Public options for ZITADEL introspection handler.
/// </summary>
public class ZitadelIntrospectionOptions : OAuth2IntrospectionOptions
{
    /// <summary>
    /// Default client assertion type that uses a bearer token to authenticate against the
    /// introspection endpoint. Used in conjunction with <see cref="JwtProfile"/>.
    /// </summary>
    public const string JwtBearerClientAssertionType = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";

    /// <summary>
    /// <para>
    /// Alternative to HTTP Basic authentication against the token endpoint.
    /// JWT Profile is recommended instead of using Client ID and Client Secret.
    /// If configured, client secret must be "null".
    /// </para>
    /// <para>
    /// If set after the "configure options" function, an event handler to update
    /// the client assertion is added.
    /// </para>
    /// </summary>
    public Application? JwtProfile { get; set; }
}
