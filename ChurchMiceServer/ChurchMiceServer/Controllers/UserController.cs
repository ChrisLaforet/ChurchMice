using ChurchMiceServer.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using ChurchMiceServer.Domains.Proxies;


namespace ChurchMiceServer.Controllers;


[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserProxy userProxy;
    private readonly ILogger<UserController> logger;

    public UserController(IUserProxy userProxy, ILogger<UserController> logger)
    {
        this.userProxy = userProxy;
    }

    [Microsoft.AspNetCore.Mvc.HttpPost("login")]
    public IActionResult Login(AuthenticateRequest model)
    {
        try
        {
            userProxy.ExpireUserTokens();

            var jwt = userProxy.AuthenticateUser(model.Username, model.Password);
            logger.Log(LogLevel.Information, "Successful login of user " + model.Username);
            var user = userProxy.GetUserByGuid(jwt.UserId);
            return Ok(new JwtResponse(jwt.Value, jwt.User, user.Email));
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, "Error logging in user " + model.Username + ": " + ex);
        }

        return BadRequest(new { message = "Unable to authenticate" });
    }
}
