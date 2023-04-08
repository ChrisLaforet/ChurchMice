using ChurchMiceServer.Security;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMiceServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    public HealthController()
    {
    }
    
    [Microsoft.AspNetCore.Mvc.HttpGet]
    public IActionResult Get()
    {
        return Ok("Services are active");
    }
}