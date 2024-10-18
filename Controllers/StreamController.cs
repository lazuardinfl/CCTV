using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCTV.ControllerS;

[ApiController]
[Route("api/[controller]")]
public class StreamController : ControllerBase
{
    [HttpGet("{id}")]
    [Authorize(Roles = "admin")]
    public string Get(string id) => id;
}
