namespace Zitadel.Authentication;

/// <summary>
/// OIDC claim types to fetch certain claims from identities.
/// </summary>
public static class OidcClaimTypes
{
    /// <summary>
    /// The "user ID" claim type of the oidc profile.
    /// </summary>
    public const string Subject = "sub";

    /// <summary>
    /// The name of the subject.
    /// </summary>
    public const string Name = "name";

    /// <summary>
    /// The given name (first name) of the subject.
    /// </summary>
    public const string GivenName = "given_name";

    /// <summary>
    /// The family name (last name) of the subject.
    /// </summary>
    public const string FamilyName = "family_name";

    /// <summary>
    /// The username of the subject.
    /// </summary>
    public const string Username = "username";

    /// <summary>
    /// The preferred username (if multiple are present) of the subject.
    /// </summary>
    public const string PreferredUsername = "preferred_username";

    /// <summary>
    /// The language code for the selected locale.
    /// </summary>
    public const string Locale = "locale";

    /// <summary>
    /// The email of the subject - if any.
    /// </summary>
    public const string Email = "email";

    /// <summary>
    /// True if the given email address is verified within the system.
    /// </summary>
    public const string EmailVerified = "email_verified";
}
