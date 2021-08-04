namespace Zitadel.Authentication.Credentials
{
    /// <summary>
    /// A private key for JWT profile authentication.
    /// </summary>
    public record JwtPrivateKey
    {
        internal JwtPrivateKey(string? path, string? content) => (Path, Content) = (path, content);

        /// <summary>
        /// Path to the key file.
        /// </summary>
#if NET5_0_OR_GREATER
        public string? Path { get; init; }
#elif NETCOREAPP3_1_OR_GREATER
        public string? Path { get; set; }
#endif

        /// <summary>
        /// Direct json content of the jwt key.
        /// </summary>
#if NET5_0_OR_GREATER
        public string? Content { get; init; }
#elif NETCOREAPP3_1_OR_GREATER
        public string? Content { get; set; }
#endif
    }

    /// <summary>
    /// JWT profile key loaded from a file path.
    /// </summary>
    public record JwtPrivateKeyPath : JwtPrivateKey
    {
        public JwtPrivateKeyPath(string path)
            : base(path, null)
        {
        }
    }

    /// <summary>
    /// JWT Profile key loaded from json content.
    /// </summary>
    public record JwtPrivateKeyContent : JwtPrivateKey
    {
        public JwtPrivateKeyContent(string content)
            : base(null, content)
        {
        }
    }
}
