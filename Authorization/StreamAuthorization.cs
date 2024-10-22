using Microsoft.AspNetCore.Authorization;

namespace CCTV.Authorization;

public class StreamRequirement(string targetQuery, string[]? additionalRoles = null) : IAuthorizationRequirement
{
    public string TargetQuery { get; } = targetQuery;
    public string[] AdditionalRoles { get; } = additionalRoles ?? [];
}

public class StreamHandler() : AuthorizationHandler<StreamRequirement>
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
                    break;
                }
            }
        }
        return Task.CompletedTask;
    }
}
