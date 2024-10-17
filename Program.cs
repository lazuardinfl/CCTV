using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;

namespace CCTV;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        var services = builder.Services;

        services.AddKeycloakWebApiAuthentication(options => {
            configuration.BindKeycloakOptions(options);
            options.AuthServerUrl = configuration.GetValue<string>("KEYCLOAK_URL") ?? options.AuthServerUrl;
        });
        services.AddAuthorization();

        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/", () => "Hello World!").RequireAuthorization();

        app.Run();
    }
}
