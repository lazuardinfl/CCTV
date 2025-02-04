# CCTV Streaming Service

CCTV streaming service with several features:
- Web UI built using SvelteKit framework.
- APIs built using ASP.NET Core framework.
- Using [go2rtc](https://github.com/AlexxIT/go2rtc) as backend interface to streaming sources.
- Keycloak for authentication and authorization.

## Deployment

ASP.NET Core Runtime and Node.js are required. Kubernetes is recommended for production use. Some environment variables are required:
- `Keycloak__auth-server-url`: Keycloak base URL, e.g. `https://keycloak.domain.com`.
- `AUTH_KEYCLOAK_ISSUER`: Keycloak base URL with realm path, e.g. `https://keycloak.domain.com/realms/<realm-name>`.
- `Keycloak__realm`: Keycloak realm name.
- `Keycloak__resource` and `AUTH_KEYCLOAK_ID`: Keycloak client id.
- `Keycloak__credentials__secret` and `AUTH_KEYCLOAK_SECRET`: Keycloak client secret.
- `APP_URL`: Web UI origin URL, e.g. `https://cctv.domain.com`.
- `APP_API_URL`: API URL, can be listening URL behind proxy, e.g. `http://localhost:8080`.
- `SVELTE_PATH_BASE`: Base path for Web UI URL, used during build only, must start with `/`, e.g. `/web`, default to `""`.

go2rtc streams [configuration](https://github.com/AlexxIT/go2rtc?tab=readme-ov-file#module-streams) is required for streaming sources.
