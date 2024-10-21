using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCTV.ControllerS;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet("{id}")]
    [Authorize(Policy = "Stream")]
    public string Get(string id) => id;
}
