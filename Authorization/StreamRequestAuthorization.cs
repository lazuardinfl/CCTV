using Microsoft.AspNetCore.Authorization;

namespace CCTV.Authorization;

public class StreamRequestRequirement(string cctv, string duration, string[]? additionalRoles = null) : IAuthorizationRequirement
{
    public string CCTV { get; } = cctv;
    public string Duration { get; } = duration;
    public string[] AdditionalRoles { get; } = additionalRoles ?? [];
}

public class StreamRequestHandler() : AuthorizationHandler<StreamRequestRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StreamRequestRequirement requirement)
    {
        try
        {
            if (context.Resource is HttpContext httpContext)
            {
                IQueryCollection queries = httpContext.Request.Query;
                List<string> roles = [.. requirement.AdditionalRoles];
                roles.Add(queries[requirement.CCTV].ToString());
                foreach (string role in roles)
                {
                    if (context.User.IsInRole(role))
                    {
                        if (IsDurationValid(role, int.Parse(queries[requirement.Duration].ToString())))
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

    private bool IsDurationValid(string role, int duration)
    {
        return role switch
        {
            "super" => true,
            "officer" when duration <= 604800 => true,
            string when duration <= 10800 => true,
            _ => false,
        };
    }
}
