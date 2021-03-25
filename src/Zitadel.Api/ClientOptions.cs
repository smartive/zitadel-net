using System.Collections.Generic;

namespace Zitadel.Api
{
    public record ClientOptions
    {
        public string Endpoint { get; set; } = string.Empty;

        public string? Token { get; set; }

        public IDictionary<string, string>? AdditionalHeaders { get; set; }
    }
}
