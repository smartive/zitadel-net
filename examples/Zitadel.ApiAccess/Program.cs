// This file contains two examples:
// 1. An example with a service account "personal access token" to access the ZITADEL API.
// 2. An example with a service account "jwt profile key" to access the ZITADEL API.

using Zitadel.Api;
using Zitadel.Credentials;

const string apiUrl = "https://zitadel-libraries-l8boqa.zitadel.cloud";
const string personalAccessToken = "ge85fvmgTX4XAhjpF0XGpelB2vn9LZanJaqmUQDuf7iTpKVowb44LFl-86pqY2mfJCEoIOk";

// or create the token provider directly:
// new StaticTokenProvider(token)
var client = Clients.AuthService(new(apiUrl, ITokenProvider.Static(personalAccessToken)));
var result = await client.GetMyUserAsync(new());
Console.WriteLine($"User: {result.User}");

var serviceAccount = ServiceAccount.LoadFromJsonString(
    @"
{
  ""type"": ""serviceaccount"",
  ""keyId"": ""170084658355110145"",
  ""key"": ""-----BEGIN RSA PRIVATE KEY-----\nMIIEpAIBAAKCAQEAnQisbU4FuLmjLR9I2Q01Rm9Mx6WySat2mbxgmOzu04oXuESI\nyS+RkiimdN0khjqouBftYqtVes7yngMLq3E8hMCwv/kLE+YeXphZXnn8tps8M2gV\n7S//uCp9LooK9qeh0lSkOqIsh0atj/l7NAHFxnhuNhfmn8XIYJNLVNSj5yzTri5E\nSn92SAsUQLSONgr7IEmIjcuPtYeU0iLvVno52ljZHnPX2WJ0HEZv44nZpkR4qBfv\n3hJzNx7sd4TdPGHHugJD8jdG/X4bAxwL5XGHZu18cUVM5RerSMpFQHSuIGgpKmK4\nWlM1AJGeut6EX/SrCxUDvhyOnXAgqhunTUmi6QIDAQABAoIBAHn7y92Y1y743X3m\nqHMbJIBTYyRPXaCGljm0MKF6o8clpWlZq5wE3KLZ+vwa8Q1oMbnXtGqKR3t/mM4P\n9Ze2/djtyh9GOUm632qCFCIkxp+fFPOl7ipyt8V7FAT77KpP6490eqKlacunppmJ\nph/vJJAY6xwQEvGX9SC4KrN5/txLKXbVtR3V2RXy9sxbbL4cpnklmRBMeXQkpwEM\nTKELUr5Rmhg9KvS3yALgVv0dIRtOA8Z995R234hXfY0St48YEvZtsxeme47u2CVl\nHJcVH4aa9Sw6XlgAEQBxqbQHpcLvUIu3XempO7VfGklWE6OlGuEcnUWpJCD8jMZW\nPYtt9LUCgYEAwi8josS3Iyto+DMJjJKCw175N2cmFMxBGu9Rw4aHjTiN57z7AUkn\nbmT44WnSmc1bCLC+nMB34vhiEyBKXYrH7zgbeMO8QDG3aO6gXdod/IdsieZR8E3b\ngUA1wtZYyRbc7eo8U4Nqkv1NXVRuDJkz/Mfoy+m1BVKcW7YeZaaZN9MCgYEAzwYB\n/LAiJoyx5UPwuieizlT7kHI7uvZRo4oLx+cZipNCJ0NGKgX4l1NIYLaNDbCoT9N0\nylico+kn+nihzDmD6SjY2hHGSIHk7AnJOcW+Bk5TfsYb8clxfgX40udLMIS0F13R\nrJt0gD9x0O3AZv4MV9cSI0/Md0tbWePgrLI44NMCgYEAojj7TlmEnY8AbIlGqvci\n4tCO5qf3elyA712LMwtKZsIeWsDX+OUCWglkmfvsAq06JfJx60YnYagbVtsdBTSR\nftmiqarrs71U+gaQVpeHgZYpKLMPNO/2Nu5Le2/SUHwXKXML3sDk4dNXNGb6YPAE\nLGNdqiyeG8o98agdkNIzIh0CgYEAlTGhMPfGRL3UXoNN8vopjEUWXozUmvJ090S/\nJLtZXtKtNBp5cEOJWZT9biVhFeKgCZc8ba7ahA29b/aLs+AnPlrfnJh+qzZhQfHz\ngJ0PSwAbkBs5fFBOaCHppiRlvXuFRemo95m4pcwTPBx7Mj4Xqx4lxij2E2rNVMSy\n4AI4l10CgYBwefqXt8B+D+0EvmhyHk19Tk8/fPelclJUv/IVI59c0F9UMAA2rD1U\nNW6k9251OGU7mQkztluNvl13qtAW/DveOjkFeDJIMzhFjravpLQXhUK4ETnM44YL\nFbClVGJaHYSHgOkNpcN5lYVLoyEvzv9rEPwBqpZRVnwWj6L+/I2L5Q==\n-----END RSA PRIVATE KEY-----\n"",
  ""userId"": ""170079991923474689""
}");
client = Clients.AuthService(
    new(
        apiUrl,
        ITokenProvider.ServiceAccount(
            apiUrl,
            serviceAccount,
            new() { ApiAccess = true })));
result = await client.GetMyUserAsync(new());
Console.WriteLine($"User: {result.User}");
