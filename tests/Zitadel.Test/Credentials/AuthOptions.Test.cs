using FluentAssertions;
using Xunit;
using Zitadel.Credentials;

namespace Zitadel.Test.Credentials;

public class AuthOptionsTest
{
    [Theory]
    [MemberData(nameof(Data))]
    public void Create_Correct_Scopes(ServiceAccount.AuthOptions options, string result)
    {
        options.CreateOidcScopes().Should().Be(result);
    }

    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new ServiceAccount.AuthOptions(), "openid" },
            new object[] { new ServiceAccount.AuthOptions { AdditionalScopes = { "foo", "bar" } }, "openid foo bar" },
            new object[]
            {
                new ServiceAccount.AuthOptions { ApiAccess = true },
                $"openid {ServiceAccount.AuthOptions.ApiAccessScope}",
            },
            new object[]
            {
                new ServiceAccount.AuthOptions { RequiredRoles = { "1234" } },
                "openid urn:zitadel:iam:org:project:role:1234",
            },
            new object[]
            {
                new ServiceAccount.AuthOptions { ProjectAudiences = { "1234" } },
                "openid urn:zitadel:iam:org:project:id:1234:aud",
            },
        };
}
