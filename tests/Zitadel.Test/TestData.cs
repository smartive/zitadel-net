using Zitadel.Credentials;

namespace Zitadel.Test;

public static class TestData
{
    public const string ApiUrl = "https://zitadel-libraries-l8boqa.zitadel.cloud";
    public const string PersonalAccessToken = "ge85fvmgTX4XAhjpF0XGpelB2vn9LZanJaqmUQDuf7iTpKVowb44LFl-86pqY2mfJCEoIOk";
    public const string OrgId = "170079925099823361";

    public const string ApplicationJson = """
                                          
                                              {
                                                "type": "application",
                                                "keyId": "170104948032274689",
                                                "key": "-----BEGIN RSA PRIVATE KEY-----\nMIIEpAIBAAKCAQEAq4+TOYQH7p/saqLYUnLJwjtT6sUCkktlkK3qqoLdn0HLmHZW\njNcHF6U2F2/HumW7RFIcmaoxxm0limS4UqNzXpqkNqF75RaSrldU5Phink5ZkTLs\nEciaaoy4eYI5rtuHhWbN6SO70eY2/XZ1TlOAL4DJbbW4p4YhKOz0Naui8r8dBqWw\nwvXdkfqLfzL43HCGES+aIaqcv99RI/wJe0ogQ+7P4enP1SeauXavBRfkEhn/wrNH\nSEbVKQ48dKdlzmNbAocsOprfs2p2muaRHlz9waqJNhdUwYEotiYhK+LeUaP8IVCJ\nN5OU/io+ifuXIXkpag3PzAQ8Fth3Au74WzzHSQIDAQABAoIBAEtoUifntqzWMk40\nwayLs87h0OLSMW0oIr5TE2BbIRqNCvY6nZRON1nXTk1C3qE5cfR3uwZ33mT/OI75\n8mKwYVdl1WQF2rU5FMP4suHpoz895PSDU2wFponKzJLsAHqxF4I1S7B7+mQqMmV6\nGdmRrjgy/VZxl3Za6FxauoSUqozTZ5vNS/1Ig+/Ri7qD4zM5HKbr7JCICuGJXPhM\nWL5CBe9sOSDtTBuzg4bp+XxVFq0mFuKlR2yG6/Ky+pDHNFxDLIrOxmSgkDayLI9r\nmxbrUGTdBiNuFe5Ezl/6WdEhzObVpxFJ4cZcCG4DH+O4F9mDasDx0wKLl+t1t7Bw\nmaur9RkCgYEA12lpRUVhsKVtREWn5I206QuCrNqkfAPJ3CcNf8YMwzsEEaWS/44V\n0oSheoHdVJ2XV56N0sLPKWJO+dqOnU2PIKoM6HeZKjKLke47+EIZaRNTrhhJzPP+\nwHIv6Jz44j2EHWi52FF4o2WqY4NS9OEWkrpapkJajfCtQAUatvUzQBMCgYEAy+L5\nMz+K9SnuKzHMFObpZobMjS/OO87wn+XLx8UItwMaergz7oCgEgSv8nYfyJ1LigHy\nkT748Ag8gLl4WCPpCMGQYfuk5zBDBW+KNU2bDZiyFNMmVd9LcbJQiGdsZIyJ9t4e\nebnZeYyYQp+j7XB4cseMKpVIHT7XqpxITSmgXrMCgYEAnzZ7J0bzwHNUwoxVXnla\niJEIYagswLiwHzcCJDmGv1nEVSKy9o3XFUUQcRLBO0RLUuiO3IM+SNEvnD5tAFkN\n+8+UQNH89BJt1EtoKcL5Mw+k3t121rRUy3rabCxxTA65sl7wVbFJ4ENJX8n1q6ce\nXw6753zNn3GPK+1Z5HZxDd8CgYAAsmXnpu/yppIJ08G+0Is7rnpEgUVTLwHjigWI\nSUQeXARbJwYGaqohZaK0UXMKXH9FmXwawvxW1bBfQEMJChZh0UeNDi8iGygffKIc\nTIebJEp3h8E5yemYGePsk23rag+OqHOyNtBnefOLRsBor1m6CrSP8LKuZuiVzLLy\nkJHbwQKBgQC9/r6I5woj+Khm2izGRAyPOhTkDbpFWr2IU5AZ3jUin5yZxXOARcUM\nv5HIVFHC/X8O4rBFfAoNGjpef99icPbIXc60hbYrCD8Q+Nky8Li7XzlqKweF8nPB\nSo9s/gLF0gZcoYxf9M9bLB5bh93P6qYGk7ybaGbe7P6aUhSHV6pZgw==\n-----END RSA PRIVATE KEY-----\n",
                                                "appId": "170101999168127233",
                                                "clientId": "170101999168192769@library"
                                              }
                                          """;

    public const string ServiceAccountJson = """
                                             
                                                 {
                                                   "type": "serviceaccount",
                                                   "keyId": "170084658355110145",
                                                   "key": "-----BEGIN RSA PRIVATE KEY-----\nMIIEpAIBAAKCAQEAnQisbU4FuLmjLR9I2Q01Rm9Mx6WySat2mbxgmOzu04oXuESI\nyS+RkiimdN0khjqouBftYqtVes7yngMLq3E8hMCwv/kLE+YeXphZXnn8tps8M2gV\n7S//uCp9LooK9qeh0lSkOqIsh0atj/l7NAHFxnhuNhfmn8XIYJNLVNSj5yzTri5E\nSn92SAsUQLSONgr7IEmIjcuPtYeU0iLvVno52ljZHnPX2WJ0HEZv44nZpkR4qBfv\n3hJzNx7sd4TdPGHHugJD8jdG/X4bAxwL5XGHZu18cUVM5RerSMpFQHSuIGgpKmK4\nWlM1AJGeut6EX/SrCxUDvhyOnXAgqhunTUmi6QIDAQABAoIBAHn7y92Y1y743X3m\nqHMbJIBTYyRPXaCGljm0MKF6o8clpWlZq5wE3KLZ+vwa8Q1oMbnXtGqKR3t/mM4P\n9Ze2/djtyh9GOUm632qCFCIkxp+fFPOl7ipyt8V7FAT77KpP6490eqKlacunppmJ\nph/vJJAY6xwQEvGX9SC4KrN5/txLKXbVtR3V2RXy9sxbbL4cpnklmRBMeXQkpwEM\nTKELUr5Rmhg9KvS3yALgVv0dIRtOA8Z995R234hXfY0St48YEvZtsxeme47u2CVl\nHJcVH4aa9Sw6XlgAEQBxqbQHpcLvUIu3XempO7VfGklWE6OlGuEcnUWpJCD8jMZW\nPYtt9LUCgYEAwi8josS3Iyto+DMJjJKCw175N2cmFMxBGu9Rw4aHjTiN57z7AUkn\nbmT44WnSmc1bCLC+nMB34vhiEyBKXYrH7zgbeMO8QDG3aO6gXdod/IdsieZR8E3b\ngUA1wtZYyRbc7eo8U4Nqkv1NXVRuDJkz/Mfoy+m1BVKcW7YeZaaZN9MCgYEAzwYB\n/LAiJoyx5UPwuieizlT7kHI7uvZRo4oLx+cZipNCJ0NGKgX4l1NIYLaNDbCoT9N0\nylico+kn+nihzDmD6SjY2hHGSIHk7AnJOcW+Bk5TfsYb8clxfgX40udLMIS0F13R\nrJt0gD9x0O3AZv4MV9cSI0/Md0tbWePgrLI44NMCgYEAojj7TlmEnY8AbIlGqvci\n4tCO5qf3elyA712LMwtKZsIeWsDX+OUCWglkmfvsAq06JfJx60YnYagbVtsdBTSR\nftmiqarrs71U+gaQVpeHgZYpKLMPNO/2Nu5Le2/SUHwXKXML3sDk4dNXNGb6YPAE\nLGNdqiyeG8o98agdkNIzIh0CgYEAlTGhMPfGRL3UXoNN8vopjEUWXozUmvJ090S/\nJLtZXtKtNBp5cEOJWZT9biVhFeKgCZc8ba7ahA29b/aLs+AnPlrfnJh+qzZhQfHz\ngJ0PSwAbkBs5fFBOaCHppiRlvXuFRemo95m4pcwTPBx7Mj4Xqx4lxij2E2rNVMSy\n4AI4l10CgYBwefqXt8B+D+0EvmhyHk19Tk8/fPelclJUv/IVI59c0F9UMAA2rD1U\nNW6k9251OGU7mQkztluNvl13qtAW/DveOjkFeDJIMzhFjravpLQXhUK4ETnM44YL\nFbClVGJaHYSHgOkNpcN5lYVLoyEvzv9rEPwBqpZRVnwWj6L+/I2L5Q==\n-----END RSA PRIVATE KEY-----\n",
                                                   "userId": "170079991923474689"
                                                 }
                                             """;

    public const string InvalidServiceAccountJson = """
                                                    
                                                        {
                                                          "type": "serviceaccount",
                                                          "keyId": "170084658355119999",
                                                          "key": "-----BEGIN RSA PRIVATE KEY-----\nMIIEpAIBAAKCAQEAnQisbU4FuLmjLR9I2Q01Rm9Mx6WySat2mbxgmOzu04oXuESI\nyS+RkiimdN0khjqouBftYqtVes7yngMLq3E8hMCwv/kLE+YeXphZXnn8tps8M2gV\n7S//uCp9LooK9qeh0lSkOqIsh0atj/l7NAHFxnhuNhfmn8XIYJNLVNSj5yzTri5E\nSn92SAsUQLSONgr7IEmIjcuPtYeU0iLvVno52ljZHnPX2WJ0HEZv44nZpkR4qBfv\n3hJzNx7sd4TdPGHHugJD8jdG/X4bAxwL5XGHZu18cUVM5RerSMpFQHSuIGgpKmK4\nWlM1AJGeut6EX/SrCxUDvhyOnXAgqhunTUmi6QIDAQABAoIBAHn7y92Y1y743X3m\nqHMbJIBTYyRPXaCGljm0MKF6o8clpWlZq5wE3KLZ+vwa8Q1oMbnXtGqKR3t/mM4P\n9Ze2/djtyh9GOUm632qCFCIkxp+fFPOl7ipyt8V7FAT77KpP6490eqKlacunppmJ\nph/vJJAY6xwQEvGX9SC4KrN5/txLKXbVtR3V2RXy9sxbbL4cpnklmRBMeXQkpwEM\nTKELUr5Rmhg9KvS3yALgVv0dIRtOA8Z995R234hXfY0St48YEvZtsxeme47u2CVl\nHJcVH4aa9Sw6XlgAEQBxqbQHpcLvUIu3XempO7VfGklWE6OlGuEcnUWpJCD8jMZW\nPYtt9LUCgYEAwi8josS3Iyto+DMJjJKCw175N2cmFMxBGu9Rw4aHjTiN57z7AUkn\nbmT44WnSmc1bCLC+nMB34vhiEyBKXYrH7zgbeMO8QDG3aO6gXdod/IdsieZR8E3b\ngUA1wtZYyRbc7eo8U4Nqkv1NXVRuDJkz/Mfoy+m1BVKcW7YeZaaZN9MCgYEAzwYB\n/LAiJoyx5UPwuieizlT7kHI7uvZRo4oLx+cZipNCJ0NGKgX4l1NIYLaNDbCoT9N0\nylico+kn+nihzDmD6SjY2hHGSIHk7AnJOcW+Bk5TfsYb8clxfgX40udLMIS0F13R\nrJt0gD9x0O3AZv4MV9cSI0/Md0tbWePgrLI44NMCgYEAojj7TlmEnY8AbIlGqvci\n4tCO5qf3elyA712LMwtKZsIeWsDX+OUCWglkmfvsAq06JfJx60YnYagbVtsdBTSR\nftmiqarrs71U+gaQVpeHgZYpKLMPNO/2Nu5Le2/SUHwXKXML3sDk4dNXNGb6YPAE\nLGNdqiyeG8o98agdkNIzIh0CgYEAlTGhMPfGRL3UXoNN8vopjEUWXozUmvJ090S/\nJLtZXtKtNBp5cEOJWZT9biVhFeKgCZc8ba7ahA29b/aLs+AnPlrfnJh+qzZhQfHz\ngJ0PSwAbkBs5fFBOaCHppiRlvXuFRemo95m4pcwTPBx7Mj4Xqx4lxij2E2rNVMSy\n4AI4l10CgYBwefqXt8B+D+0EvmhyHk19Tk8/fPelclJUv/IVI59c0F9UMAA2rD1U\nNW6k9251OGU7mQkztluNvl13qtAW/DveOjkFeDJIMzhFjravpLQXhUK4ETnM44YL\nFbClVGJaHYSHgOkNpcN5lYVLoyEvzv9rEPwBqpZRVnwWj6L+/I2L5Q==\n-----END RSA PRIVATE KEY-----\n",
                                                          "userId": "170079991923479999"
                                                        }
                                                    """;

    public static Application Application => Application.LoadFromJsonString(ApplicationJson);

    public static ServiceAccount ServiceAccount => ServiceAccount.LoadFromJsonString(ServiceAccountJson);
}
