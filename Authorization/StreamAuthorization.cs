using CCTV.Services;
using Microsoft.AspNetCore.Authorization;

namespace CCTV.Authorization;

public class StreamRequirement(string cctv, string token) : IAuthorizationRequirement
{
    public string CCTV { get; } = cctv;
    public string Token { get; } = token;
}

public class StreamHandler(StreamToken streamToken) : AuthorizationHandler<StreamRequirement>
{
    protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, StreamRequirement requirement)
    {
        try
        {
            if (context.Resource is HttpContext httpContext)
            {
                string cctv = httpContext.Request.Query[requirement.CCTV].ToString();
                string token = httpContext.Request.Query[requirement.Token].ToString();
                if ((await streamToken.ValidateToken(token)) && (cctv == streamToken.GetTokenClaim(token, requirement.CCTV)))
                {
                    context.Succeed(requirement);
                }
            }
        }
        catch {}
        return Task.CompletedTask;
    }
}
