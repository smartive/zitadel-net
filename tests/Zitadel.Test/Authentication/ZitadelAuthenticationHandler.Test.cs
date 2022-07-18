using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Zitadel.Credentials;
using Zitadel.Test.WebFactories;

namespace Zitadel.Test.Authentication;

public class ZitadelAuthenticationHandlerTest : IClassFixture<AuthenticationHandlerWebFactory>
{
    private readonly AuthenticationHandlerWebFactory _factory;

    public ZitadelAuthenticationHandlerTest(AuthenticationHandlerWebFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Should_Be_Able_To_Call_Unauthorized_Endpoint()
    {
        var client = _factory.CreateClient();
        var result =
            await client.GetFromJsonAsync("/unauthed", typeof(AuthenticationHandlerWebFactory.Unauthed)) as
                AuthenticationHandlerWebFactory.Unauthed;
        result.Should().NotBeNull();
        result?.Ping.Should().Be("Pong");
    }

    [Fact]
    public async Task Should_Return_Unauthorized_Without_Any_Token()
    {
        var client = _factory.CreateClient();
        var result = await client.GetAsync("/authed");
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // TODO: fix this test.
    // [Fact]
    // public async Task Should_Return_Data_With_Token()
    // {
    //     var serviceAccount = ServiceAccount.LoadFromJsonString(
    //         @"
    //         {
    //           ""type"": ""serviceaccount"",
    //           ""keyId"": ""170084658355110145"",
    //           ""key"": ""-----BEGIN RSA PRIVATE KEY-----\nMIIEpAIBAAKCAQEAnQisbU4FuLmjLR9I2Q01Rm9Mx6WySat2mbxgmOzu04oXuESI\nyS+RkiimdN0khjqouBftYqtVes7yngMLq3E8hMCwv/kLE+YeXphZXnn8tps8M2gV\n7S//uCp9LooK9qeh0lSkOqIsh0atj/l7NAHFxnhuNhfmn8XIYJNLVNSj5yzTri5E\nSn92SAsUQLSONgr7IEmIjcuPtYeU0iLvVno52ljZHnPX2WJ0HEZv44nZpkR4qBfv\n3hJzNx7sd4TdPGHHugJD8jdG/X4bAxwL5XGHZu18cUVM5RerSMpFQHSuIGgpKmK4\nWlM1AJGeut6EX/SrCxUDvhyOnXAgqhunTUmi6QIDAQABAoIBAHn7y92Y1y743X3m\nqHMbJIBTYyRPXaCGljm0MKF6o8clpWlZq5wE3KLZ+vwa8Q1oMbnXtGqKR3t/mM4P\n9Ze2/djtyh9GOUm632qCFCIkxp+fFPOl7ipyt8V7FAT77KpP6490eqKlacunppmJ\nph/vJJAY6xwQEvGX9SC4KrN5/txLKXbVtR3V2RXy9sxbbL4cpnklmRBMeXQkpwEM\nTKELUr5Rmhg9KvS3yALgVv0dIRtOA8Z995R234hXfY0St48YEvZtsxeme47u2CVl\nHJcVH4aa9Sw6XlgAEQBxqbQHpcLvUIu3XempO7VfGklWE6OlGuEcnUWpJCD8jMZW\nPYtt9LUCgYEAwi8josS3Iyto+DMJjJKCw175N2cmFMxBGu9Rw4aHjTiN57z7AUkn\nbmT44WnSmc1bCLC+nMB34vhiEyBKXYrH7zgbeMO8QDG3aO6gXdod/IdsieZR8E3b\ngUA1wtZYyRbc7eo8U4Nqkv1NXVRuDJkz/Mfoy+m1BVKcW7YeZaaZN9MCgYEAzwYB\n/LAiJoyx5UPwuieizlT7kHI7uvZRo4oLx+cZipNCJ0NGKgX4l1NIYLaNDbCoT9N0\nylico+kn+nihzDmD6SjY2hHGSIHk7AnJOcW+Bk5TfsYb8clxfgX40udLMIS0F13R\nrJt0gD9x0O3AZv4MV9cSI0/Md0tbWePgrLI44NMCgYEAojj7TlmEnY8AbIlGqvci\n4tCO5qf3elyA712LMwtKZsIeWsDX+OUCWglkmfvsAq06JfJx60YnYagbVtsdBTSR\nftmiqarrs71U+gaQVpeHgZYpKLMPNO/2Nu5Le2/SUHwXKXML3sDk4dNXNGb6YPAE\nLGNdqiyeG8o98agdkNIzIh0CgYEAlTGhMPfGRL3UXoNN8vopjEUWXozUmvJ090S/\nJLtZXtKtNBp5cEOJWZT9biVhFeKgCZc8ba7ahA29b/aLs+AnPlrfnJh+qzZhQfHz\ngJ0PSwAbkBs5fFBOaCHppiRlvXuFRemo95m4pcwTPBx7Mj4Xqx4lxij2E2rNVMSy\n4AI4l10CgYBwefqXt8B+D+0EvmhyHk19Tk8/fPelclJUv/IVI59c0F9UMAA2rD1U\nNW6k9251OGU7mQkztluNvl13qtAW/DveOjkFeDJIMzhFjravpLQXhUK4ETnM44YL\nFbClVGJaHYSHgOkNpcN5lYVLoyEvzv9rEPwBqpZRVnwWj6L+/I2L5Q==\n-----END RSA PRIVATE KEY-----\n"",
    //           ""userId"": ""170079991923474689""
    //         }");
    //     var token = await serviceAccount.AuthenticateAsync(
    //         new("https://zitadel-libraries-l8boqa.zitadel.cloud") { ProjectAudiences = { "170078979166961921" } });
    //     var client = _factory.CreateClient();
    //     client.DefaultRequestHeaders.Authorization = new("Bearer", token);
    //     var result = await client.GetAsync("/authed");
    //     result.StatusCode.Should().Be(HttpStatusCode.OK);
    // }
}
