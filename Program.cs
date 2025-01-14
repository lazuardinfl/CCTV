using CCTV.Authorization;
using CCTV.Services;
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
                options.AuthServerUrl = configuration["KEYCLOAK_URL"] ?? options.AuthServerUrl;
            });
        services.AddAuthorization()
            .AddKeycloakAuthorization(options => {
                options.EnableRolesMapping = RolesClaimTransformationSource.ResourceAccess;
                options.RolesResource = configuration.GetKeycloakOptions<KeycloakInstallationOptions>()?.Resource;
            })
            .AddAuthorizationBuilder()
            .AddPolicy("Admin", policy => policy.RequireRole("super"))
            .AddPolicy("Stream", policy => {
                policy.AddRequirements(new StreamRequirement("src", "token", "duration"));
            })
            .AddPolicy("StreamRequest", policy => {
                policy.AddRequirements(new StreamRequestRequirement("src"));
            });
        services.AddControllers();
        services.AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"));
        services.AddRequestTimeouts(options => {
            options.AddPolicy("StreamNormal", TimeSpan.FromHours(1));
            options.AddPolicy("StreamExtended", TimeSpan.FromDays(3));
        });
        services.AddSingleton<StreamToken>();
        services.AddSingleton<IAuthorizationHandler, StreamHandler>();
        services.AddSingleton<IAuthorizationHandler, StreamRequestHandler>();

        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRequestTimeouts();
        app.MapControllers();
        app.MapReverseProxy();

        app.Run();
    }
}
