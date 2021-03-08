using Microsoft.AspNetCore.Authorization;
using Zitadel.Authentication;

namespace Zitadel.Authorization
{
    /// <summary>
    /// Extensions for <see cref="AuthorizationOptions"/>.
    /// </summary>
    public static class AuthorizationOptionsExtensions
    {
        /// <summary>
        /// Add a policy to the options that require an organization
        /// role to be present for the user. This can be used if some application
        /// provides multi-tenancy and the service must be able to distinguish roles
        /// from different Zitadel organizations.
        /// </summary>
        /// <param name="options">The options object to extend.</param>
        /// <param name="policyName">Name of the policy (e.g. CaosUser).</param>
        /// <param name="organizationId">The id of the organization from Zitadel.</param>
        /// <param name="roles">A list of roles that need to be fulfilled (one of them, at least).</param>
        public static void AddZitadelOrganizationRolePolicy(
            this AuthorizationOptions options,
            string policyName,
            string organizationId,
            params string[] roles) =>
            options.AddPolicy(
                policyName,
                policy => policy.RequireClaim(ZitadelDefaults.OrganizationRoleClaimName(organizationId), roles));
    }
}
