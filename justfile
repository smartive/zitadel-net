#!/usr/bin/env just --justfile

default: clean generate-grpc

# Keep this in sync with `Zitadel.Api.ZitadelGrpcVersion.SupportedTag`.
zitadel_tag := "v4.15.0"

@clean:
    rm -rf ./src/Zitadel/Api/Generated

generate-grpc:
    buf generate https://github.com/zitadel/zitadel.git#ref={{zitadel_tag}},depth=1 --include-imports --path ./proto/zitadel
