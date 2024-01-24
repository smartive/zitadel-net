using System.Security.Claims;

using Zitadel.Extensions;

namespace Zitadel.Authentication;

/// <summary>
/// Additional custom claim types for ZITADEL.
/// </summary>
public static class ZitadelClaimTypes
{
    /// <summary>
    /// The type (name) for the token claim (JWT or opaque via user-info endpoint)
    /// that contains rule allocations.
    /// </summary>
    public const string Role = "urn:zitadel:iam:org:project:roles";

    /// <summary>
    /// The type (name) for the token claim (JWT or opaque via user-info endpoint)
    /// that contains the scoped and used primary domain.
    /// </summary>
    public const string PrimaryDomain = "urn:zitadel:iam:org:domain:primary";

    /// <summary>
    /// Constructor for organisation specific role claims.
    /// They are used to specify roles on a specific organization.
    /// Check for those roles with the policies added with
    /// <see cref="AuthorizationOptionsExtensions.AddZitadelOrganizationRolePolicy"/> or
    /// inside a method with
    /// <see cref="ClaimsPrincipalExtensions.IsInRole(ClaimsPrincipal,string,string[])"/>.
    /// </summary>
    /// <param name="orgId">The id of the organization.</param>
    /// <returns>A role name.</returns>
    public static string OrganizationRole(string orgId) => $"urn:zitadel:iam:org:{orgId}:project:roles";
}
