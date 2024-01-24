using Microsoft.AspNetCore.Authentication;

namespace Zitadel.Authentication.Options
{
    internal class LocalFakeZitadelSchemeOptions : AuthenticationSchemeOptions
    {
        public LocalFakeZitadelOptions FakeZitadelOptions { get; set; } = new();
    }
}
