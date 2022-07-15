using Zitadel.Credentials;

namespace Zitadel.Api;

/// <summary>
/// Token provider for gRPC clients. These providers allow the usage of
/// gRPC clients without the need to provide a token for every call.
/// </summary>
public interface ITokenProvider
{
    /// <summary>
    /// Create a static token provider (<see cref="StaticTokenProvider"/>).
    /// </summary>
    /// <param name="token">The access token to use for authentication.</param>
    /// <returns>A static <see cref="ITokenProvider"/>.</returns>
    static ITokenProvider Static(string token) => new StaticTokenProvider(token);

    /// <summary>
    /// Create a service account token provider(<see cref="ServiceAccountTokenProvider"/>).
    /// </summary>
    /// <param name="serviceAccount">The service account used for authentication.</param>
    /// <param name="apiUrl">The url on which the ZITADEL API is reachable (also this is used as audience).</param>
    /// <param name="apiProjectId">
    /// The ZITADEL API project id (id of the "ZITADEL" project in the instance). This is required
    /// to access the API with the given token. Otherwise the token will be rejected (invalid audience error).
    /// </param>
    /// <returns>A <see cref="ITokenProvider"/> that uses a service account for authentication.</returns>
    static ITokenProvider ServiceAccount(ServiceAccount serviceAccount, string apiUrl, string apiProjectId) =>
        new ServiceAccountTokenProvider(serviceAccount, new(apiUrl) { ProjectAudiences = { apiProjectId } });

    internal DelegatingHandler CreateHandler();
}
