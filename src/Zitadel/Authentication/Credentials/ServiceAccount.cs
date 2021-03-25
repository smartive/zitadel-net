﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Jose;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Zitadel.Authentication.Credentials
{
    /// <summary>
    /// <para>
    /// A Zitadel <see cref="ServiceAccount"/> can be loaded from a json file
    /// and helps with authentication on a Zitadel IAM.
    /// </para>
    /// <para>
    /// The mechanism is defined here:
    /// <a href="https://docs.zitadel.ch/architecture/#JSON_Web_Token_JWT_Profile">Zitadel Docs</a>
    /// </para>
    /// </summary>
    public record ServiceAccount
    {
        private const string DiscoveryEndpointPath = "/.well-known/openid-configuration";
        private static readonly HttpClient HttpClient = new();

        /// <summary>
        /// The key type.
        /// </summary>
        public const string Type = "serviceaccount";

        /// <summary>
        /// The user id associated with this service account.
        /// </summary>
        public string UserId { get; init; } = string.Empty;

        /// <summary>
        /// This is unique ID (on Zitadel) of the key.
        /// </summary>
        public string KeyId { get; init; } = string.Empty;

        /// <summary>
        /// The private key generated by Zitadel for this <see cref="ServiceAccount"/>.
        /// </summary>
        public string Key { get; init; } = string.Empty;

        /// <summary>
        /// Load an <see cref="ServiceAccount"/> from a file at a given (relative or absolute) path.
        /// </summary>
        /// <param name="pathToJson">The relative or absolute filepath to the json file.</param>
        /// <returns>The parsed <see cref="ServiceAccount"/>.</returns>
        /// <exception cref="FileNotFoundException">When the file does not exist.</exception>
        /// <exception cref="InvalidDataException">When the deserializer returns 'null'.</exception>
        /// <exception cref="System.Text.Json.JsonException">
        /// Thrown when the JSON is invalid,
        /// the <see cref="ServiceAccount"/> type is not compatible with the JSON,
        /// or when there is remaining data in the Stream.
        /// </exception>
        public static async Task<ServiceAccount> LoadFromJsonFileAsync(string pathToJson)
        {
            var path = Path.GetFullPath(
                Path.IsPathRooted(pathToJson)
                    ? pathToJson
                    : Path.Join(Directory.GetCurrentDirectory(), pathToJson));

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File not found: {path}.", path);
            }

            await using var stream = File.OpenRead(path);
            return await LoadFromJsonStreamAsync(stream);
        }

        /// <inheritdoc cref="LoadFromJsonFileAsync"/>
        public static ServiceAccount LoadFromJsonFile(string pathToJson) => LoadFromJsonFileAsync(pathToJson).Result;

        /// <summary>
        /// Load a <see cref="ServiceAccount"/> from a given stream (FileStream, MemoryStream, ...).
        /// </summary>
        /// <param name="stream">The stream to read the json from.</param>
        /// <returns>The parsed <see cref="ServiceAccount"/>.</returns>
        /// <exception cref="InvalidDataException">When the deserializer returns 'null'.</exception>
        /// <exception cref="System.Text.Json.JsonException">
        /// Thrown when the JSON is invalid,
        /// the <see cref="ServiceAccount"/> type is not compatible with the JSON,
        /// or when there is remaining data in the Stream.
        /// </exception>
        public static async Task<ServiceAccount> LoadFromJsonStreamAsync(Stream stream) =>
            await JsonSerializer.DeserializeAsync<ServiceAccount>(
                stream,
                new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }) ??
            throw new InvalidDataException("The json file yielded a 'null' result for deserialization.");

        /// <inheritdoc cref="LoadFromJsonStreamAsync"/>
        public static ServiceAccount LoadFromJsonStream(Stream stream) => LoadFromJsonStreamAsync(stream).Result;

        /// <summary>
        /// Load a <see cref="ServiceAccount"/> from a string that contains json.
        /// </summary>
        /// <param name="json">Json string.</param>
        /// <returns>The parsed <see cref="ServiceAccount"/>.</returns>
        /// <exception cref="InvalidDataException">When the deserializer returns 'null'.</exception>
        /// <exception cref="System.Text.Json.JsonException">
        /// Thrown when the JSON is invalid,
        /// the <see cref="ServiceAccount"/> type is not compatible with the JSON,
        /// or when there is remaining data in the Stream.
        /// </exception>
        public static async Task<ServiceAccount> LoadFromJsonStringAsync(string json)
        {
            await using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json), 0, json.Length);
            return await LoadFromJsonStreamAsync(memoryStream);
        }

        /// <inheritdoc cref="LoadFromJsonStringAsync"/>
        public static ServiceAccount LoadFromJsonString(string json) => LoadFromJsonStringAsync(json).Result;

        public async Task<string> AuthenticateAsync(string endpoint, string? discoveryEndpoint = null)
        {
            var manager = new ConfigurationManager<OpenIdConnectConfiguration>(
                DiscoveryEndpoint(discoveryEndpoint ?? endpoint),
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever(HttpClient));

            var oidcConfig = await manager.GetConfigurationAsync();

            var jwt = await GetSignedJwtAsync(endpoint);
            var request = new HttpRequestMessage(HttpMethod.Post, oidcConfig.TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string?, string?>("grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer"),
                        new KeyValuePair<string?, string?>(
                            "assertion",
                            $"{jwt}"),
                    }),
            };

            var response = await HttpClient.SendAsync(request);
            var token = await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<AccessTokenResponse>();

            return token?.AccessToken ?? throw new("Access token could not be parsed.");
        }

        private static string DiscoveryEndpoint(string discoveryEndpoint) =>
            discoveryEndpoint.EndsWith(DiscoveryEndpointPath)
                ? discoveryEndpoint
                : discoveryEndpoint.TrimEnd('/') + DiscoveryEndpointPath;

        private string GetSignedJwt(string issuer) => GetSignedJwtAsync(issuer).Result;

        private async Task<string> GetSignedJwtAsync(string issuer)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(await GetRsaParametersAsync());

            return JWT.Encode(
                new Dictionary<string, object>
                {
                    { "iss", UserId },
                    { "sub", UserId },
                    { "iat", DateTimeOffset.Now.ToUnixTimeSeconds() },
                    { "exp", ((DateTimeOffset)DateTime.Now.AddMinutes(1)).ToUnixTimeSeconds() },
                    { "aud", issuer },
                },
                rsa,
                JwsAlgorithm.RS256,
                new Dictionary<string, object>
                {
                    { "kid", KeyId },
                });
        }

        private async Task<RSAParameters> GetRsaParametersAsync()
        {
            var bytes = Encoding.UTF8.GetBytes(Key);
            await using var ms = new MemoryStream(bytes);
            using var sr = new StreamReader(ms);
            var pemReader = new PemReader(sr);

            if (!(pemReader.ReadObject() is AsymmetricCipherKeyPair keyPair))
            {
                throw new("RSA Keypair could not be read.");
            }

            return DotNetUtilities.ToRSAParameters(keyPair.Private as RsaPrivateCrtKeyParameters);
        }

        private record AccessTokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; init; } = string.Empty;
        }
    }
}
