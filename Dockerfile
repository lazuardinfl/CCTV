# syntax=docker/dockerfile:1

### START DEV
FROM mcr.microsoft.com/devcontainers/dotnet:8.0 AS dev

# install custom root CA
COPY ca/* /usr/local/share/ca-certificates/
RUN update-ca-certificates
### END DEV

### START BUILD DOTNET
FROM mcr.microsoft.com/dotnet/sdk:8.0.405 AS build-dotnet
WORKDIR /app
COPY . .
RUN rm *.sln package.json package-lock.json tsconfig.json
RUN dotnet restore
RUN dotnet publish -o out
### END BUILD DOTNET

### START PROD DOTNET
FROM mcr.microsoft.com/dotnet/aspnet:8.0.12 AS prod-dotnet

# install custom root CA
COPY ca/* /usr/local/share/ca-certificates/
RUN update-ca-certificates

# copy app files
WORKDIR /app
COPY --from=build-dotnet --chown=app:app /app/out .

USER app:app
EXPOSE 8080
ENTRYPOINT ["dotnet", "CCTV.dll"]
### END PROD DOTNET
