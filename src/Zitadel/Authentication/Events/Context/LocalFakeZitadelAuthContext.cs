using System.Security.Claims;

namespace Zitadel.Authentication.Events.Context
{
    public class LocalFakeZitadelAuthContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="identity">The created ClaimsIdentity.</param>
        public LocalFakeZitadelAuthContext(ClaimsIdentity identity)
        {
            Identity = identity;
        }

        /// <summary>
        /// The created ClaimsIdentity.
        /// </summary>
        public ClaimsIdentity Identity { get; init; }

        /// <summary>
        /// The claims of the created ClaimsIdentity.
        /// </summary>
        public IEnumerable<Claim> Claims => Identity.Claims;

        /// <summary>
        /// The "user-id" of the fake user.
        /// Either set by the options or via HTTP header.
        /// </summary>
        public string FakeZitadelId => new ClaimsPrincipal(Identity).FindFirstValue("sub")!;

        /// <summary>
        /// Add a claim to the <see cref="Claims"/> list.
        /// This is a convenience method for modifying <see cref="Claims"/>.
        /// </summary>
        /// <param name="type">Type of the claim (examples: <see cref="ClaimTypes"/>).</param>
        /// <param name="value">The value.</param>
        /// <param name="valueType">Type of the value (examples: <see cref="ClaimValueTypes"/>).</param>
        /// <param name="issuer">The issuer for this claim.</param>
        /// <param name="originalIssuer">The original issuer of this claim.</param>
        /// <returns>The <see cref="LocalFakeZitadelAuthContext"/> for chaining.</returns>
        public LocalFakeZitadelAuthContext AddClaim(
            string type,
            string value,
            string? valueType = null,
            string? issuer = null,
            string? originalIssuer = null) => AddClaim(new(type, value, valueType, issuer, originalIssuer));

        /// <summary>
        /// Add a claim to the <see cref="Claims"/> list.
        /// This is a convenience method for modifying <see cref="Claims"/>.
        /// </summary>
        /// <param name="claim">The claim to add.</param>
        /// <returns>The <see cref="LocalFakeZitadelAuthContext"/> for chaining.</returns>
        public LocalFakeZitadelAuthContext AddClaim(Claim claim)
        {
            Identity.AddClaim(claim);
            return this;
        }

        /// <summary>
        /// Add a single role to the identity's claims.
        /// Note: the roles are actually "claims" but this method exists
        /// for convenience.
        /// </summary>
        /// <param name="role">The role to add.</param>
        /// <returns>The <see cref="LocalFakeZitadelAuthContext"/> for chaining.</returns>
        public LocalFakeZitadelAuthContext AddRole(string role)
        {
            Identity.AddClaim(new(ClaimTypes.Role, role));
            return this;
        }

        /// <summary>
        /// Add multiple roles to the identity's claims.
        /// Note: the roles are actually "claims" but this method exists
        /// for convenience.
        /// </summary>
        /// <param name="roles">The roles to add.</param>
        /// <returns>The <see cref="LocalFakeZitadelAuthContext"/> for chaining.</returns>
        public LocalFakeZitadelAuthContext AddRoles(string[] roles)
        {
            foreach (var role in roles)
            {
                AddRole(role);
            }

            return this;
        }
    }
}
