using Zitadel.Credentials;

namespace Zitadel.Authentication.Options;

/// <summary>
/// Public options for ZITADEL introspection handler.
/// </summary>
public class ZitadelIntrospectionOptions // OAuth2Introspectionoptions
{
    /// <summary>
    /// Default client assertion type that uses a bearer token to authenticate against the
    /// introspection endpoint. Used in conjunction with <see cref="JwtProfile"/>.
    /// </summary>
    public const string JwtBearerClientAssertionType = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";

    /// <summary>
    /// Defines the authority for the token.
    /// </summary>
    public string Authority { get; set; } = string.Empty;

    /// <summary>
    /// The client id for the token verification.
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// The client secret for the token verification.
    /// This must be set if HTTP Basic authentication is used (for the introspection).
    /// It is recommended to use <see cref="JwtProfile"/>.
    /// </summary>
    public string? ClientSecret { get; set; }

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

    /// <summary>
    /// Defines if the received audiences should be validated or not.
    /// </summary>
    public bool ValidateAudience { get; set; } = true;

    /// <summary>
    /// List of valid audiences to check for.
    /// If <see cref="ValidateAudience"/> is `true` and this
    /// list is not defined, it defaults to a list with one entry.
    /// The entry is the value of <see cref="ClientId"/>.
    /// </summary>
    public IEnumerable<string>? ValidAudiences { get; set; }

    /// <summary>
    /// The well-known discovery endpoint. This is used for opaque token
    /// validation to determine the user-info endpoint via discovery document.
    /// If omitted, the <see cref="Authority"/> is used with the well-known endpoint.
    /// </summary>
    public string? DiscoveryEndpoint { get; set; }

    /// <summary>
    /// The primary domain - if any - that the user must belong to.
    /// </summary>
    public string? PrimaryDomain { get; set; }
}
