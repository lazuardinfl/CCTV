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

### START BUILD NODE.JS
FROM node:20.18.2-slim AS build-node

ARG SVELTE_PATH_BASE

WORKDIR /app
COPY package*.json .
RUN npm ci
COPY . .
RUN npm run build
RUN npm prune --production
### END BUILD NODE.JS

### START PROD NODE.JS
FROM node:20.18.2-slim AS prod-node

ENV NODE_ENV=production
ENV AUTH_KEYCLOAK_ISSUER=https://www.keycloak.org/realms/name

# install custom root CA
RUN apt-get update && apt-get install --no-install-recommends -y ca-certificates \
    && apt-get clean && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*
COPY ca/* /usr/local/share/ca-certificates/
RUN update-ca-certificates

# copy app files
WORKDIR /app
COPY --chown=node:node package.json .
COPY --from=build-node --chown=node:node /app/build build/
COPY --from=build-node --chown=node:node /app/node_modules node_modules/

USER node:node
EXPOSE 3000
CMD ["node", "build"]
### END PROD NODE.JS
