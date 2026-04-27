using FluentAssertions;

using Xunit;

using ZitadelApplication = Zitadel.Credentials.Application;

namespace Zitadel.Test.Credentials;

public class ApplicationTest
{
    [Fact]
    public async Task Load_App_From_Json()
    {
        var app = await ZitadelApplication.LoadFromJsonStringAsync(TestData.ApplicationJson);
        app.AppId.Should().Be("170101999168127233");
    }

    [Fact]
    public async Task Create_Signed_Jwt()
    {
        var app = await ZitadelApplication.LoadFromJsonStringAsync(TestData.ApplicationJson);
        var token = await app.GetSignedJwtAsync(TestData.ApiUrl);

        token.Should().StartWith("ey");
    }
}
