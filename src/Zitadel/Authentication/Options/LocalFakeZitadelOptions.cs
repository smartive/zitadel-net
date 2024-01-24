using System.Security.Claims;

namespace Zitadel.Authentication.Options
{
    public class LocalFakeZitadelOptions
    {
        /// <summary>
        /// The "user-id" of the fake user.
        /// This populates the "sub" and "nameidentifier" claims.
        /// </summary>
        public string FakeZitadelId { get; set; } = string.Empty;

        /// <summary>
        /// A list of additional claims to add to the identity.
        /// </summary>
        public IList<Claim> AdditionalClaims { get; set; } = new List<Claim>();

        /// <summary>
        /// List of roles that are attached to the identity.
        /// Note: the roles are actually "claims" but this list exists
        /// for convenience.
        /// </summary>
        public IEnumerable<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// Add a claim to the <see cref="AdditionalClaims"/> list.
        /// This is a convenience method for modifying <see cref="AdditionalClaims"/>.
        /// </summary>
        /// <param name="type">Type of the claim (examples: <see cref="ClaimTypes"/>).</param>
        /// <param name="value">The value.</param>
        /// <param name="valueType">Type of the value (examples: <see cref="ClaimValueTypes"/>).</param>
        /// <param name="issuer">The issuer for this claim.</param>
        /// <param name="originalIssuer">The original issuer of this claim.</param>
        /// <returns>The <see cref="LocalFakeZitadelOptions"/> for chaining.</returns>
        public LocalFakeZitadelOptions AddClaim(
            string type,
            string value,
            string? valueType = null,
            string? issuer = null,
            string? originalIssuer = null) => AddClaim(new(type, value, valueType, issuer, originalIssuer));

        /// <summary>
        /// Add a claim to the <see cref="AdditionalClaims"/> list.
        /// This is a convenience method for modifying <see cref="AdditionalClaims"/>.
        /// </summary>
        /// <param name="claim">The claim to add.</param>
        /// <returns>The <see cref="LocalFakeZitadelOptions"/> for chaining.</returns>
        public LocalFakeZitadelOptions AddClaim(Claim claim)
        {
            AdditionalClaims.Add(claim);
            return this;
        }
    }
}
