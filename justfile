#!/usr/bin/env just --justfile

default: clean generate-grpc

@clean:
    rm -rf ./src/Zitadel.Grpc/Zitadel
    rm -rf ./src/Zitadel.Grpc/Google
    rm -rf ./src/Zitadel.Grpc/Grpc
    rm -rf ./src/Zitadel.Grpc/Validate

generate-grpc:
    buf generate https://github.com/zitadel/zitadel.git#depth=1 --include-imports --path ./proto/zitadel
