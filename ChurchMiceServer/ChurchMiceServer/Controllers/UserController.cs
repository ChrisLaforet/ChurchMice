using ChurchMiceServer.Controllers.Models;
using ChurchMiceServer.CQS.CommandHandlers;
using ChurchMiceServer.CQS.Commands;
using Microsoft.AspNetCore.Mvc;
using ChurchMiceServer.Domains.Proxies;


namespace ChurchMiceServer.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserProxy userProxy;
    private readonly ILogger<UserController> logger;

    private readonly LoginCommandHandler loginCommandHandler;
    private readonly SetPasswordCommandHandler setPasswordCommandHandler;

    public UserController(IServiceProvider serviceProvider, IUserProxy userProxy, ILogger<UserController> logger)
    {
        this.userProxy = userProxy;
        this.logger = logger;
        this.loginCommandHandler = ActivatorUtilities.CreateInstance<LoginCommandHandler>(serviceProvider);
        this.setPasswordCommandHandler = ActivatorUtilities.CreateInstance<SetPasswordCommandHandler>(serviceProvider);
    }

    [Microsoft.AspNetCore.Mvc.HttpPost("login")]
    public IActionResult Login(AuthenticateRequest model)
    {
        try
        {
            var response = loginCommandHandler.Handle(new LoginCommand(model.Username, model.Password));
            var user = userProxy.GetUserByGuid(response.Token.UserId);
            return Ok(new JwtResponse(response.Token.Value, response.Token.User, user.Email));
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Debug, "Error logging in user " + model.Username + ": " + ex);
        }

        return BadRequest(new { message = "Unable to authenticate" });
    }
    
    [Microsoft.AspNetCore.Mvc.HttpPost("setPassword")]
    public IActionResult SetPassword(SetPasswordRequest model)
    {
        try
        {
            setPasswordCommandHandler.Handle(new SetPasswordCommand(model.Username, model.ResetKey, model.Password));
            return Ok("Password set");
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Debug, "Error setting password for user " + model.Username + ": " + ex);
        }

        return BadRequest(new { message = "Unable to set password" });
    }
}
