using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;

namespace CCTV.Authorization;

public class StreamRequirement(string targetQuery, string[]? additionalRoles = null) : IAuthorizationRequirement
{
    public string TargetQuery { get; } = targetQuery;
    public string[] AdditionalRoles { get; } = additionalRoles ?? [];
}

public class StreamHandler(ILogger<StreamRequirement> logger) : AuthorizationHandler<StreamRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StreamRequirement requirement)
    {
        if (context.Resource is HttpContext httpContext)
        {
            List<string> roles = [.. requirement.AdditionalRoles];
            roles.Add(httpContext.Request.Query[requirement.TargetQuery].ToString());
            foreach (string role in roles)
            {
                if (context.User.IsInRole(role))
                {
                    context.Succeed(requirement);
                    logger.LogInformation("IS IN ROLE {url}", httpContext.Request.GetEncodedPathAndQuery());
                    break;
                }
            }
        }
        return Task.CompletedTask;
    }
}
