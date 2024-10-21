using CCTV.Authorization;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace CCTV;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        var services = builder.Services;

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
            .AddAuthorizationBuilder()
            .AddPolicy("Stream", policy => {
                policy.AddRequirements(new StreamRequirement());
            });
        services.AddControllers();
        services.AddSingleton<IAuthorizationHandler, StreamHandler>();

        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.MapGet("/", () => "Hello World!").RequireAuthorization();

        app.Run();
    }
}
