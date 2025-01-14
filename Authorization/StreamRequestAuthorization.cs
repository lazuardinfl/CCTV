using Microsoft.AspNetCore.Authorization;

namespace CCTV.Authorization;

public class StreamRequestRequirement(string cctv) : IAuthorizationRequirement
{
    public string CCTV { get; } = cctv;
}

public class StreamRequestHandler() : AuthorizationHandler<StreamRequestRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StreamRequestRequirement requirement)
    {
        try
        {
            if (context.Resource is HttpContext httpContext)
            {
                List<string> roles = ["super", "officer"];
                roles.Add(httpContext.Request.Query[requirement.CCTV].ToString());
                foreach (string role in roles)
                {
                    if (context.User.IsInRole(role))
                    {
                        context.Succeed(requirement);
                        break;
                    }
                }
            }
        }
        catch {}
        return Task.CompletedTask;
    }
}
