using System.Text;
using System.Web;

namespace Zitadel.Credentials;

/// <summary>
/// Credentials for basic authentication (url encoded).
/// <param name="ClientId">The HTTP Basic client id.</param>
/// <param name="ClientSecret">The HTTP Basic client secret.</param>
/// </summary>
public record BasicAuthentication(string ClientId, string ClientSecret)
{
    private const string BasicAuthPrefix = "Basic";

    /// <summary>
    /// Basic credentials used as HTTP header.
    /// </summary>
    public string HttpCredentials =>
        $"{BasicAuthPrefix} {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{HttpUtility.UrlEncode(ClientId)}:{HttpUtility.UrlEncode(ClientSecret)}"))}";
}
