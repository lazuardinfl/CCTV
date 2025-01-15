using CCTV.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CCTV.ControllerS;

[ApiController]
[Route("api/[controller]")]
public class AuthController(StreamToken streamToken) : ControllerBase
{
    [HttpPost("stream/token")]
    [Authorize(Policy = "StreamRequest")]
    public string GenerateStreamToken([FromQuery] string src, [FromQuery] string duration = "normal")
        => streamToken.GenerateToken(
            DateTime.Now.AddHours(1),
            new ClaimsIdentity([
                new Claim("src", src),
                new Claim("duration", duration)
            ])
        );
}
