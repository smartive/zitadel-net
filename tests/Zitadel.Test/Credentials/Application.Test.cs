using FluentAssertions;

using Xunit;

using Zitadel.Credentials;

namespace Zitadel.Test.Credentials;

public class ApplicationTest
{
    [Fact]
    public async Task Load_App_From_Json()
    {
        var app = await Application.LoadFromJsonStringAsync(TestData.ApplicationJson);
        app.AppId.Should().Be("170101999168127233");
    }

    [Fact]
    public async Task Create_Signed_Jwt()
    {
        var app = await Application.LoadFromJsonStringAsync(TestData.ApplicationJson);
        var token = await app.GetSignedJwtAsync(TestData.ApiUrl);

        token.Should().StartWith("ey");
    }
}
