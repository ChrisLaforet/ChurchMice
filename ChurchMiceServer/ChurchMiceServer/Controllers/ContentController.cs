using ChurchMiceServer.Controllers.Models;
using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.QueryHandlers;
using ChurchMiceServer.Security;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMiceServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContentController : ControllerBase
{
	private readonly ILogger<ContentController> logger;
    
	public ContentFileListQueryHandler ContentFileListQueryHandler { get; set; }
	public ContentFileQueryHandler ContentFileQueryHandler { get; set; }

	public ContentController(IServiceProvider serviceProvider, ILogger<ContentController> logger)
	{
		this.logger = logger;

		if (serviceProvider != null)
		{
			this.ContentFileListQueryHandler = ActivatorUtilities.CreateInstance<ContentFileListQueryHandler>(serviceProvider);
			this.ContentFileQueryHandler = ActivatorUtilities.CreateInstance<ContentFileQueryHandler>(serviceProvider);
		}
	}

	[HttpGet]
	[Authorize]
	public ContentListResponse Get()
	{
		try
		{
			var response = ContentFileListQueryHandler.Handle(new ContentFileListQuery());
			return new ContentListResponse()
			{
				About = response.About,
				Index = response.Index,
				Beliefs = response.Beliefs,
				Services = response.Services,
				Logo = response.Logo
			};
		}
		catch (Exception ex)
		{
			logger.Log(LogLevel.Debug, "Error retrieving user content page list", ex);
		}

		return new ContentListResponse();
	}

	[HttpGet("getContent")]
	[Authorize]
	public IActionResult GetContent(string key)
	{
		try
		{
			var fileContent = ContentFileQueryHandler.Handle(new ContentFileQuery(key));
			if (fileContent != null)
			{
				return File(fileContent.Content, fileContent.MimeType, fileContent.FileName);
			}
		}
		catch (Exception ex)
		{
			logger.Log(LogLevel.Debug, $"Error retrieving user content for key of {key}", ex);
		}

		return NotFound();
	}
}