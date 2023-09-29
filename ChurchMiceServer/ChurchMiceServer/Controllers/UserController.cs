﻿using ChurchMiceServer.Controllers.Models;
using ChurchMiceServer.CQS.CommandHandlers;
using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.Domains.Models;
using Microsoft.AspNetCore.Mvc;
using ChurchMiceServer.Domains.Proxies;
using Microsoft.AspNetCore.Authorization;


namespace ChurchMiceServer.Controllers;


[Route("api/[controller]")]
[ApiController]
public partial class UserController : ControllerBase
{
    private readonly IUserProxy userProxy;
    private readonly ILogger<UserController> logger;

    public LoginCommandHandler LoginCommandHandler { get; set; }
    public SetPasswordCommandHandler SetPasswordCommandHandler { get; set; }
    public ChangePasswordCommandHandler ChangePasswordCommandHandler { get; set; }
    public LogoutCommandHandler LogoutCommandHandler { get; set; }
    public CheckExistingNameCommandHandler CheckExistingNameCommandHandler { get; set; }
    public CreateUserCommandHandler CreateUserCommandHandler { get; set; }
    public ValidateUserEmailCommandHandler ValidateUserEmailCommandHandler { get; set; }


    public UserController(IServiceProvider? serviceProvider, IUserProxy userProxy, ILogger<UserController> logger)
    {
        this.userProxy = userProxy;
        this.logger = logger;
        if (serviceProvider != null)
        {
            this.LoginCommandHandler = ActivatorUtilities.CreateInstance<LoginCommandHandler>(serviceProvider);
            this.SetPasswordCommandHandler = ActivatorUtilities.CreateInstance<SetPasswordCommandHandler>(serviceProvider);
            this.ChangePasswordCommandHandler = ActivatorUtilities.CreateInstance<ChangePasswordCommandHandler>(serviceProvider);
            this.LogoutCommandHandler = ActivatorUtilities.CreateInstance<LogoutCommandHandler>(serviceProvider);
            this.CheckExistingNameCommandHandler = ActivatorUtilities.CreateInstance<CheckExistingNameCommandHandler>(serviceProvider);
            this.CreateUserCommandHandler = ActivatorUtilities.CreateInstance<CreateUserCommandHandler>(serviceProvider);
            this.ValidateUserEmailCommandHandler = ActivatorUtilities.CreateInstance<ValidateUserEmailCommandHandler>(serviceProvider);
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login(AuthenticateRequest model)
    {
        try
        {
            var response = LoginCommandHandler.Handle(new LoginCommand(model.Username, model.Password));
            var user = userProxy.GetUserByGuid(response.Token.UserId);
            return Ok(new JwtResponse(response.Token.Value, response.Token.User,  user.Fullname, user.Email));
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Debug, "Error logging in user " + model.Username + ": " + ex);
        }

        return BadRequest(new { message = "Unable to authenticate" });
    }
    
    [HttpPost("completePasswordChange")]
    [AllowAnonymous]
    public IActionResult CompletePasswordChange(CompletePasswordChangeRequest model)
    {
        try
        {
            SetPasswordCommandHandler.Handle(new SetPasswordCommand(model.Username, model.ResetKey, model.Password));
            return Ok(new {message = "Password has been set"});
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Error setting password for user {model.Username}: {ex.ToString()}");
        }

        return BadRequest(new { message = "Unable to set password" });
    }
    
    [HttpPut("logout")]
    [AllowAnonymous]
    public IActionResult Logout()
    {
        try
        {
            var possibleUser = this.Request.HttpContext.Items["User"];
            if (possibleUser != null)
            {
                User user = (User) possibleUser;
                LogoutCommandHandler.Handle(new LogoutCommand(user.Username));
            }
        }
        catch (Exception ex)
        {
            logger.LogDebug($"Exception caught while attempting to log out: {ex}");
        }

        return Ok(new {message = "Logged out"});
    }

    [HttpPost("requestPasswordChange")]
    [AllowAnonymous]
    public IActionResult RequestPasswordChange(ChangePasswordRequest model)
    {
        ChangePasswordCommandHandler.Handle(new ChangePasswordCommand(model.UserName));
        return Ok(new {message = "Password change sent in Email"});
    }
    
    [HttpPost("checkExistingName")]
    [AllowAnonymous]
    public IActionResult CheckExistingName(CheckExistingNameRequest model)
    {
        if (CheckExistingNameCommandHandler.Handle(new CheckExistingNameCommand(model.CheckField, model.CheckValue)).Value)
        {
            return Ok(new {message = "Value is available"});
        }

        return BadRequest(new { message = "Value is already used" });
    }
    
    [HttpPost("createUser")]
    [AllowAnonymous]
    public IActionResult CreateUser(CreateUserRequest model)
    {
        if (!CheckExistingNameCommandHandler.Handle(new CheckExistingNameCommand("USERNAME", model.UserName)).Value)
        {
            return BadRequest(new {message = "Requested user name is not permitted"});
        }

        try
        {
            CreateUserCommandHandler.Handle(new CreateUserCommand(model.UserName, model.Password, model.Email, model.FullName));
            return Ok(new {message = "Check your Email (and Spam filters) to complete creating your account"});
        }
        catch (Exception ex)
        {
            return BadRequest(new {message = "Something went wrong while creating user account"});
        }
    }
    
    [HttpPost("validateUserEmail")]
    [AllowAnonymous]
    public IActionResult ValidateUserEmail(ValidateUserEmailRequest model)
    {
        try
        {
            ValidateUserEmailCommandHandler.Handle(new ValidateUserEmailCommand(model.UserName, model.Password));
            return Ok(new {message = "Email address for user account has been validated"});
        }
        catch (Exception ex)
        {
            return BadRequest(new {message = "Something went wrong while validating Email address for user account"});
        }
    }
}
