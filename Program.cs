using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CCTV;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        var services = builder.Services;

        services.AddControllers();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddKeycloakWebApi(options => {
                configuration.BindKeycloakOptions(options);
                options.AuthServerUrl = configuration.GetValue<string>("KEYCLOAK_URL") ?? options.AuthServerUrl;
            });

        services.AddAuthorization()
            .AddKeycloakAuthorization(options => {
                options.EnableRolesMapping = RolesClaimTransformationSource.ResourceAccess;
                options.RolesResource = configuration.GetKeycloakOptions<KeycloakInstallationOptions>()?.Resource;
            })
            .AddAuthorizationBuilder();

        var app = builder.Build();

        app.MapControllers();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/", () => "Hello World!").RequireAuthorization();

        app.Run();
    }
}
