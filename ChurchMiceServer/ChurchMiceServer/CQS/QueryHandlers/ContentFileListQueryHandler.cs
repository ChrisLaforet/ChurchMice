using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Exceptions;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.QueryHandlers;

public class ContentFileListQueryHandler : IQueryHandler<ContentFileListQuery, ContentFileListResponse>
{
	private readonly IConfigurationProxy configurationProxy;
	private readonly ILogger<ContentFileListQueryHandler> logger;

	public ContentFileListQueryHandler(IConfigurationProxy configurationProxy, ILogger<ContentFileListQueryHandler> logger)
	{
		this.configurationProxy = configurationProxy;
		this.logger = logger;
	}

	public ContentFileListResponse Handle(ContentFileListQuery query)
	{
		try
		{
			return DetermineAvailableContentIn(configurationProxy.GetUserContentPath());
		}
		catch (NotConfiguredException nce)
		{
			logger.LogInformation("There is no content file path configured");
		}
		return new ContentFileListResponse();
	}

	private ContentFileListResponse DetermineAvailableContentIn(string path)
	{
		var response = new ContentFileListResponse();
		if (Directory.Exists(path))
		{
			foreach (var file in Directory.GetFiles(path))
			{
				var fileName = Path.GetFileName(file);
				if (fileName == null)
				{
					continue;
				}
				switch (fileName.ToLower())
				{
					case "logo.png":
						response.Logo = true;
						break;
					case "index.html":
						response.Index = true;
						break;
					case "about.html":
						response.About = true;
						break;
					case "services.html":
						response.Services = true;
						break;
					case "beliefs.html":
						response.Beliefs = true;
						break;
				}
			}
		}
		else
		{
			logger.LogError($"The configured path {path} does not exist.  Check the path or create the directory.");
		}

		return response;
	}
}