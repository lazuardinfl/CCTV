# syntax=docker/dockerfile:1

## start dev
FROM mcr.microsoft.com/devcontainers/dotnet:8.0 AS dev

# install custom root CA
COPY ca/* /usr/local/share/ca-certificates/
RUN update-ca-certificates
## end dev

## start prod
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS prod
## end prod
