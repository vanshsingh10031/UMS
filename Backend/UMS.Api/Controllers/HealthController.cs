using Microsoft.AspNetCore.Mvc;

namespace UMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "ok",
            service = "UMS.Api",
            timestamp = DateTimeOffset.UtcNow
        });
    }
}
