using ChurchMiceServer.Security;
using ChurchMiceServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMiceServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    // private IEmailSenderService emailSenderService;
    //
    // public HealthController(IEmailSenderService emailSenderService)
    // {
    //     this.emailSenderService = emailSenderService;
    // }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Get()
    {
//        emailSenderService.SendSingleMessageTo("laforet@chrislaforetsoftware.com", "Email testing", "SendGrid email body test of a message send");
        return Ok("Services are active");
    }
}