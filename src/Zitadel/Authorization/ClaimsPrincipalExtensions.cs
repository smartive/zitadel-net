using System.Linq;
using System.Security.Claims;
using Zitadel.Authentication;

namespace Zitadel.Authorization
{
    /// <summary>
    /// Extensions for <see cref="ClaimsPrincipal"/>.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Checks a principal if it inherits one of the provided roles.
        /// </summary>
        /// <param name="principal">The principal to check.</param>
        /// <param name="roles">List of roles. One of them must be present on the principal.</param>
        /// <returns>True if any role is on the principal. False otherwise.</returns>
        public static bool IsInRole(this ClaimsPrincipal principal, params string[] roles) =>
            roles.Select(principal.IsInRole).Any();

        /// <summary>
        /// Checks a principal if it inherits a specific role in context of an organization.
        /// </summary>
        /// <param name="principal">The principal to check.</param>
        /// <param name="organizationId">Zitadel ID of the organization.</param>
        /// <param name="role">Role that must be present on the principal.</param>
        /// <returns>True if the role is on the principal. False otherwise.</returns>
        public static bool IsInRole(this ClaimsPrincipal principal, string organizationId, string role) =>
            principal.HasClaim(ZitadelDefaults.OrganizationRoleClaimName(organizationId), role);

        /// <summary>
        /// Checks a principal if it inherits one or more of the provided roles in context of an organization.
        /// </summary>
        /// <param name="principal">The principal to check.</param>
        /// <param name="organizationId">Zitadel ID of the organization.</param>
        /// <param name="roles">List of roles. One of them must be present on the principal.</param>
        /// <returns>True if any role is on the principal. False otherwise.</returns>
        public static bool IsInRole(this ClaimsPrincipal principal, string organizationId, params string[] roles)
            => roles.Select(role => IsInRole(principal, organizationId, role)).Any();
    }
}
