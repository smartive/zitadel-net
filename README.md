# ZITADEL .NET

[![.NET Release](https://github.com/smartive/zitadel-net/actions/workflows/dotnet-release.yml/badge.svg)](https://github.com/smartive/zitadel-net/actions/workflows/dotnet-release.yml)
[![Nuget](https://img.shields.io/nuget/v/Zitadel)](https://www.nuget.org/packages/Zitadel/)

Welcome to the repository of the ZITADEL dotnet libraries.

This repository contains authentication and resource management for ZITADEL in .NET.
It can be used to create a ASP.NET application (with internal session management)
or WebAPIs with OIDC introspection. Further, the compiled proto resources of the
ZITADEL source repository are included to access the API of ZITADEL and manage resources.

as well as the [examples](./examples) folder which contains several examples
for accessing the [API of ZITADEL](./examples/Zitadel.ApiAccess)
or using it in a [WebApp](./examples/Zitadel.AspNet.AuthN) or
[WebAPI](./examples/Zitadel.WebApi).

### Development

To help developing the libraries, you may just open an issue or create a pull request
to this repository.

To set up the dev environment you merely only need to
install the [.NET SDK](https://dotnet.microsoft.com/download) and you are good to go.

##### License

These libraries are licensed under the [Apache 2.0 License](LICENSE).
