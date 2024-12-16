# syntax=docker/dockerfile:1

### START DEV
FROM mcr.microsoft.com/devcontainers/dotnet:8.0 AS dev

# install custom root CA
COPY ca/* /usr/local/share/ca-certificates/
RUN update-ca-certificates
### END DEV

### START PROD
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS prod
### END PROD
