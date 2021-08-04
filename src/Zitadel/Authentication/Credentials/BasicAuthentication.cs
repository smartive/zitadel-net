using System;
using System.Text;
using System.Web;

namespace Zitadel.Authentication.Credentials
{
#if NET5_0_OR_GREATER
    /// <summary>
    /// Credentials for basic authentication for OIDC (url encoded).
    /// </summary>
    public record BasicAuthentication(string ClientId, string ClientSecret)
    {
        private const string BasicAuthPrefix = "Basic";

        /// <summary>
        /// Create the basic credentials used as HTTP header.
        /// </summary>
        public string HttpCredentials =>
            $"{BasicAuthPrefix} {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{HttpUtility.UrlEncode(ClientId)}:{HttpUtility.UrlEncode(ClientSecret)}"))}";
    }
#elif NETCOREAPP3_1_OR_GREATER
    /// <summary>
    /// Credentials for basic authentication for OIDC (url encoded).
    /// </summary>
    public class BasicAuthentication
    {
        private const string BasicAuthPrefix = "Basic";
        private readonly string _clientId;
        private readonly string _clientSecret;

        public BasicAuthentication(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        /// <summary>
        /// Create the basic credentials used as HTTP header.
        /// </summary>
        public string HttpCredentials =>
            $"{BasicAuthPrefix} {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{HttpUtility.UrlEncode(_clientId)}:{HttpUtility.UrlEncode(_clientSecret)}"))}";
    }
#endif
}
