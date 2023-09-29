using ChurchMiceServer.Controllers.Models;
using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.QueryHandlers;
using ChurchMiceServer.Security;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMiceServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
	private readonly ILogger<AdminController> logger;
    
	public LocalConfigurationQueryHandler LocalConfigurationQueryHandler { get; set; }

	public AdminController(IServiceProvider serviceProvider, ILogger<AdminController> logger)
	{
		this.logger = logger;

		if (serviceProvider != null)
		{
			this.LocalConfigurationQueryHandler = ActivatorUtilities.CreateInstance<LocalConfigurationQueryHandler>(serviceProvider);
		}
	}

	[HttpGet]
	public ConfigurationValuesResponse GetLocalConfiguration()
	{
		return new ConfigurationValuesResponse(LocalConfigurationQueryHandler.Handle(new LocalConfigurationQuery()));
	}

	[HttpPut("setLocalConfigurationValue")]
	[Authorize(Roles = "Administrator")]
	public IActionResult SetLocalConfigurationValue()
	{
		throw new NotImplementedException();
	}
}