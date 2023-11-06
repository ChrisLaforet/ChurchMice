using ChurchMiceServer.Controllers.Models;
using ChurchMiceServer.CQS.CommandHandlers;
using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.QueryHandlers;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Security;
using ChurchMiceServer.Types;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMiceServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
	private readonly ILogger<AdminController> logger;
    
	public LocalConfigurationQueryHandler LocalConfigurationQueryHandler { get; set; }
	public SetLocalConfigurationCommandHandler SetLocalConfigurationCommandHandler { get; set; }
	public GetUsersQueryHandler GetUsersQueryHandler { get; set; }
	public SetUserRoleCommandHandler SetUserRoleCommandHandler { get; set; }
	public GetUserQueryHandler GetUserQueryHandler { get; set; }
	public CheckExistingNameQueryHandler CheckExistingNameQueryHandler { get; set; }
	public CreateUserCommandHandler CreateUserCommandHandler { get; set; }
	public SaveUserCommandHandler SaveUserCommandHandler { get; set; }
	public SetUserPasswordCommandHandler SetUserPasswordCommandHandler { get; set; }

	public AdminController(IServiceProvider serviceProvider, ILogger<AdminController> logger)
	{
		this.logger = logger;

		if (serviceProvider != null)
		{
			this.LocalConfigurationQueryHandler = ActivatorUtilities.CreateInstance<LocalConfigurationQueryHandler>(serviceProvider);
			this.SetLocalConfigurationCommandHandler = ActivatorUtilities.CreateInstance<SetLocalConfigurationCommandHandler>(serviceProvider);
			this.GetUsersQueryHandler = ActivatorUtilities.CreateInstance<GetUsersQueryHandler>(serviceProvider);
			this.SetUserRoleCommandHandler = ActivatorUtilities.CreateInstance<SetUserRoleCommandHandler>(serviceProvider);
			this.GetUserQueryHandler = ActivatorUtilities.CreateInstance<GetUserQueryHandler>(serviceProvider);
			this.CheckExistingNameQueryHandler = ActivatorUtilities.CreateInstance<CheckExistingNameQueryHandler>(serviceProvider);
			this.CreateUserCommandHandler = ActivatorUtilities.CreateInstance<CreateUserCommandHandler>(serviceProvider);
			this.SaveUserCommandHandler = ActivatorUtilities.CreateInstance<SaveUserCommandHandler>(serviceProvider);
			this.SetUserPasswordCommandHandler = ActivatorUtilities.CreateInstance<SetUserPasswordCommandHandler>(serviceProvider);
		}
	}

	[HttpGet("getLocalConfiguration")]
	public ConfigurationValuesResponse GetLocalConfiguration()
	{
		return new ConfigurationValuesResponse(LocalConfigurationQueryHandler.Handle(new LocalConfigurationQuery()));
	}

	[HttpPut("setLocalConfiguration")]
	[Authorize(Roles = "Administrator")]
	public IActionResult SetLocalConfiguration(LocalConfigurationChangeRequest request)
	{
		try
		{
			SetLocalConfigurationCommandHandler.Handle(new SetLocalConfigurationCommand(request.MinistryName, request.BaseUrl, request.MinistryAddress1,
																request.MinistryAddress2, request.MinistryAddress3, request.MinistryPhone,
																request.FacebookUrl, request.YouTubeUrl, request.VimeoUrl, request.InstagramUrl));
			return Ok(new {message = "Changes completed successfully"});
		}
		catch (Exception ex)
		{
			return BadRequest(new {message = "Error changing one or more configuration items"});
		}
	}
	
	[HttpGet("getUsers")]
	[Authorize(Roles = "Administrator")]
	public UserResponse[] GetUsers()
	{
		return GetUsersQueryHandler.Handle(new GetUsersQuery()).Users;
	}
	
	[HttpGet("getUser/{userId}")]
	[Authorize(Roles = "Administrator")]
	public UserResponse? GetUser(string userId)
	{
		return GetUserQueryHandler.Handle(new GetUserQuery(UserId.From(userId)));
	}

	[HttpPut("setUserRole")]
	[Authorize(Roles = "Administrator")]
	public IActionResult SetUserRole(UserRoleRequest request)
	{
		try
		{
			SetUserRoleCommandHandler.Handle(new SetUserRoleCommand(UserId.From(request.UserId), RoleLevelCode.From(request.RoleLevelCode)));
			return Ok(new {message = "Role changed successfully"});
		}
		catch (Exception)
		{
			return BadRequest(new {message = "User role has not been changed"});
		}
	}
	
	[HttpPost("createUser")]
	[Authorize(Roles = "Administrator")]
	public IActionResult CreateUser(CreateUserRequest model)
	{
		if (!CheckExistingNameQueryHandler.Handle(new CheckExistingNameQuery("USERNAME", model.UserName)).Value)
		{
			return BadRequest(new {message = "Requested user name is not permitted"});
		}

		try
		{
			var userId = CreateUserCommandHandler.Handle(new CreateUserCommand(model.UserName, model.FullName, model.Email));
			return Ok(new {message = "Created new user", other = userId.Value});
		}
		catch (Exception ex)
		{
			return BadRequest(new {message = "Something went wrong while creating user account"});
		}
	}
	
	[HttpPut("updateUser")]
	[Authorize(Roles = "Administrator")]
	public IActionResult UpdateUser(UpdateUserRequest model)
	{
		try
		{
			SaveUserCommandHandler.Handle(new SaveUserCommand(UserId.From(model.UserId), model.FullName, model.Email));
			return Ok(new {message = "Updated user"});
		}
		catch (Exception ex)
		{
			return BadRequest(new {message = "Something went wrong while creating user account"});
		}
	}
	
	[HttpPut("setUserPassword")]
	[Authorize(Roles = "Administrator")]
	public IActionResult SetUserPassword(UserPasswordRequest request)
	{
		try
		{
			SetUserPasswordCommandHandler.Handle(new SetUserPasswordCommand(UserId.From(request.UserId), request.Password));
			return Ok(new {message = "Password changed successfully"});
		}
		catch (Exception)
		{
			return BadRequest(new {message = "User password has not been changed"});
		}
	}
}