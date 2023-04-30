using ChurchMiceServer.Controllers.Models;
using ChurchMiceServer.CQS.CommandHandlers;
using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.Domains.Models;
using Microsoft.AspNetCore.Mvc;
using ChurchMiceServer.Domains.Proxies;
using ChurchMiceServer.Security;


namespace ChurchMiceServer.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserProxy userProxy;
    private readonly ILogger<UserController> logger;

    private readonly LoginCommandHandler loginCommandHandler;
    private readonly SetPasswordCommandHandler setPasswordCommandHandler;
    private readonly ChangePasswordCommandHandler changePasswordCommandHandler;
    private readonly LogoutCommandHandler logoutCommandHandler;

    public UserController(IServiceProvider serviceProvider, IUserProxy userProxy, ILogger<UserController> logger)
    {
        this.userProxy = userProxy;
        this.logger = logger;
        this.loginCommandHandler = ActivatorUtilities.CreateInstance<LoginCommandHandler>(serviceProvider);
        this.setPasswordCommandHandler = ActivatorUtilities.CreateInstance<SetPasswordCommandHandler>(serviceProvider);
        this.changePasswordCommandHandler = ActivatorUtilities.CreateInstance<ChangePasswordCommandHandler>(serviceProvider);
        this.logoutCommandHandler = ActivatorUtilities.CreateInstance<LogoutCommandHandler>(serviceProvider);
    }

    [HttpPost("login")]
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
    
    [HttpPost("setPassword")]
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
    
    [Authorize]
    [HttpPut("logout")]
    public IActionResult Logout()
    {
        try
        {
            var possibleUser = this.Request.HttpContext.Items["User"];
            if (possibleUser != null)
            {
                User user = (User) possibleUser;
                logoutCommandHandler.Handle(new LogoutCommand(user.Username));
            }
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Debug, "Exception caught while attempting to log out: " + ex);
        }

        return Ok("Logged out");
    }

    [HttpPost("changePassword")]
    public IActionResult ChangePassword(ChangePasswordRequest model)
    {
        changePasswordCommandHandler.Handle(new ChangePasswordCommand(model.Email));
        return Ok("Password change sent in Email");
    }
}
