# ZITADEL .NET

[![.NET Release](https://github.com/zitadel/zitadel-net/actions/workflows/dotnet-release.yml/badge.svg)](https://github.com/zitadel/zitadel-net/actions/workflows/dotnet-release.yml)
[![Code Security Testing](https://github.com/zitadel/zitadel-net/actions/workflows/security-analysis.yml/badge.svg)](https://github.com/zitadel/zitadel-net/actions/workflows/security-analysis.yml)
[![Nuget](https://img.shields.io/nuget/v/Zitadel)](https://www.nuget.org/packages/Zitadel/)

Welcome to the repository of the ZITADEL dotnet libraries.

This repository contains two essential ZITADEL libraries:

- [ZITADEL](./src/Zitadel)
- [ZITADEL.gRPC](./src/Zitadel.Grpc)

While Zitadel.gRPC is only the compiled proto files from the [ZITADEL source repository](https://github.com/zitadel/zitadel),
the other lib contains various helpers and extensions to support ZITADEL.

Both libraries contain a readme file to document the library and the usage itself
as well as the [examples](./examples) folder which contains several examples
for accessing the [API of ZITADEL](./examples/Zitadel.ApiAccess)
or using it in a [WebApp](./examples/Zitadel.AspNet.AuthN) or
[WebAPI](./examples/Zitadel.WebApi).

### Development

To help developing the libraries, you may just open an issue or create a pull request
to this repository.

To set up the dev environment you merely only need to:

- use `git submodule update --init` to initialize the submodules
- install the [.NET SDK](https://dotnet.microsoft.com/download)

The build directory uses ["nuke"](https://nuke.build/) to build the libraries.

##### License

These libraries are licensed under the [Apache 2.0 License](LICENSE).
