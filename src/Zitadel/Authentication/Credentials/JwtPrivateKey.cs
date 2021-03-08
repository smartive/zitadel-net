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
        public string? Path { get; init; }

        /// <summary>
        /// Direct json content of the jwt key.
        /// </summary>
        public string? Content { get; init; }
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
