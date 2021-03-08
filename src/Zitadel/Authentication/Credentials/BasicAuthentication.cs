using System;
using System.Text;
using System.Web;

namespace Zitadel.Authentication.Credentials
{
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
}
