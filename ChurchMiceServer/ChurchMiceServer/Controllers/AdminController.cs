using ChurchMiceServer.Controllers.Models;
using ChurchMiceServer.CQS.CommandHandlers;
using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.QueryHandlers;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Security;
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

	public AdminController(IServiceProvider serviceProvider, ILogger<AdminController> logger)
	{
		this.logger = logger;

		if (serviceProvider != null)
		{
			this.LocalConfigurationQueryHandler = ActivatorUtilities.CreateInstance<LocalConfigurationQueryHandler>(serviceProvider);
			this.SetLocalConfigurationCommandHandler = ActivatorUtilities.CreateInstance<SetLocalConfigurationCommandHandler>(serviceProvider);
			this.GetUsersQueryHandler = ActivatorUtilities.CreateInstance<GetUsersQueryHandler>(serviceProvider);
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
			SetLocalConfigurationCommandHandler.Handle(new SetLocalConfigurationCommand(request.MinistryName, request.BaseUrl));
			return Ok(new {message = "Changes completed successfully"});
		}
		catch (Exception)
		{
			return BadRequest(new {message = "Error changing one or more configuration items"});
		}
	}
	
	[HttpGet("getUsers")]
	[Authorize(Roles = "Administrator")]
	public UsersResponse GetUsers()
	{
		return GetUsersQueryHandler.Handle(new GetUsersQuery());
	}
}