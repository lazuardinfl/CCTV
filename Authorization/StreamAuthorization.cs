using CCTV.Services;
using Microsoft.AspNetCore.Authorization;

namespace CCTV.Authorization;

public class StreamRequirement(string cctv, string token, string duration) : IAuthorizationRequirement
{
    public string CCTV { get; } = cctv;
    public string Token { get; } = token;
    public string Duration { get; } = duration;
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
                string duration = httpContext.Request.Query[requirement.Duration].ToString();
                if ((await streamToken.ValidateToken(token)) &&
                    (cctv == streamToken.GetTokenClaim(token, requirement.CCTV)) &&
                    IsDurationValid(duration, token, requirement.Duration))
                {
                    context.Succeed(requirement);
                }
            }
        }
        catch {}
        return Task.CompletedTask;
    }

    private bool IsDurationValid(string duration, string token, string claimName)
    {
        string? claim = streamToken.GetTokenClaim(token, claimName);
        string[] normal = ["normal", "extended", "unlimited"];
        string[] extended = ["extended", "unlimited"];
        string[] unlimited = ["unlimited"];
        return duration switch
        {
            "normal" => normal.Contains(claim),
            "extended" => extended.Contains(claim),
            "unlimited" => unlimited.Contains(claim),
            _ => false,
        };
    }
}
