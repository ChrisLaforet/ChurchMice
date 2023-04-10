using ChurchMiceServer.Security;
using ChurchMiceServer.Services;
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
    
    [Microsoft.AspNetCore.Mvc.HttpGet]
    public IActionResult Get()
    {
//        emailSenderService.SendSingleMessageTo("laforet@chrislaforetsoftware.com", "Email testing", "SendGrid email body test of a message send");
        return Ok("Services are active");
    }
}