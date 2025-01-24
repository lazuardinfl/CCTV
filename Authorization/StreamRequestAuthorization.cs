using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
                        if (IsDurationValid(duration, context.User))
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

    private bool IsDurationValid(string duration, ClaimsPrincipal user)
    {
        var durations = new Dictionary<string, string[]>()
        {
            { "normal", ["normal", "extended", "unlimited", "super"] },
            { "extended", ["extended", "unlimited", "super"] },
            { "unlimited", ["unlimited", "super"] }
        };
        foreach (string role in durations[duration])
        {
            if (user.IsInRole(role))
            {
                return true;
            }
        }
        return false;
    }
}
