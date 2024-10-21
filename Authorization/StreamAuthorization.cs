using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;

namespace CCTV.Authorization;

public class StreamRequirement : IAuthorizationRequirement
{
}

public class StreamHandler(ILogger<StreamRequirement> logger) : AuthorizationHandler<StreamRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StreamRequirement requirement)
    {
        if (context.Resource is HttpContext httpContext)
        {
            logger.LogInformation("URL {url}", httpContext.Request.GetEncodedUrl());
            logger.LogInformation("ROLE {role}", context.User.IsInRole("stream role"));
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}
