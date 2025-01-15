using Microsoft.AspNetCore.Authorization;

namespace CCTV.Authorization;

public class StreamRequestRequirement(string cctv, string duration) : IAuthorizationRequirement
{
    public string CCTV { get; } = cctv;
    public string Duration { get; } = duration;
}

public class StreamRequestHandler() : AuthorizationHandler<StreamRequestRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StreamRequestRequirement requirement)
    {
        try
        {
            if (context.Resource is HttpContext httpContext)
            {
                string duration = httpContext.Request.Query[requirement.Duration].ToString();
                duration = string.IsNullOrEmpty(duration) ? "normal" : duration;
                List<string> roles = ["super", "officer"];
                roles.Add(httpContext.Request.Query[requirement.CCTV].ToString());
                foreach (string role in roles)
                {
                    if (context.User.IsInRole(role))
                    {
                        if (IsDurationValid(duration, role))
                        {
                            context.Succeed(requirement);
                        }
                        break;
                    }
                }
            }
        }
        catch {}
        return Task.CompletedTask;
    }

    private bool IsDurationValid(string duration, string role)
    {
        string[] normal = ["normal"];
        string[] extended = ["normal", "extended"];
        return role switch
        {
            "super" => true,
            "officer" => extended.Contains(duration),
            string => normal.Contains(duration),
            _ => false,
        };
    }
}
