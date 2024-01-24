#!/usr/bin/env just --justfile

default: clean generate-grpc

@clean:
    rm -rf ./src/Zitadel/Api/Generated

generate-grpc:
    buf generate https://github.com/zitadel/zitadel.git#depth=1 --include-imports --path ./proto/zitadel
