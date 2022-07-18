# ZITADEL gRPC

This library contains the compiled gRPC client definitions of
ZITADEL. The original [protos](https://github.com/zitadel/zitadel/tree/v2-alpha/proto/zitadel) reside in the
[ZITADEL repository](https://github.com/zitadel/zitadel) and are
included via git submodules.

All protos from the source repository are built within this library.
However, this package only contains the compiled protos and no further
implementation or convenience methods. To access those, head over to
the [ZITADEL package](../Zitadel/).

To use the bare gRPC client, import the `Zitadel.Grpc` package
into your project.

The code below shows an example on how to use the bare gRPC clients
to access the ZITADEL API:

```csharp
var accessToken = "fetch it somehow";
var channel = GrpcChannel.ForAddress("https://my-zitadel-api.com");
var client = new AuthService.AuthServiceClient(channel);
var result = await client.GetMyUserAsync(
    new(),
    new Metadata { { "Authorization", $"Bearer {accessToken}" } });
Console.WriteLine($"User: {result.User}");
```

All other clients/calls are documented in the
[ZITADEL docs](https://docs.zitadel.com/docs/apis/introduction).
