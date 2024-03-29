﻿using ChurchMiceServer.CQS.Exceptions;
using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Exceptions;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.QueryHandlers;

public class ContentFileQueryHandler : IQueryHandler<ContentFileQuery, FileContentResponse?>
{
	private readonly IConfigurationProxy configurationProxy;
	private readonly ILogger<ContentFileQueryHandler> logger;

	public ContentFileQueryHandler(IConfigurationProxy configurationProxy, ILogger<ContentFileQueryHandler> logger)
	{
		this.configurationProxy = configurationProxy;
		this.logger = logger;
	}
	
	public FileContentResponse? Handle(ContentFileQuery query)
	{
		try
		{
			return FetchFileContentFor(query.Key);
		}
		catch (NotConfiguredException nce)
		{
			logger.LogInformation("There is no content file path configured");
		}
		catch (InvalidContentFileKeyException icfe)
		{
			logger.LogInformation(icfe.Message);
		}
		catch (Exception e)
		{
			logger.LogInformation($"Unable to open/read contents of file for {query.Key} because of {e.Message}");
		}

		return null;
	}

	private FileContentResponse? FetchFileContentFor(string key)
	{
		var path = configurationProxy.GetUserContentPath();
		var fileName = GetFilenameFor(key);
		var content = File.ReadAllBytes(Path.Combine(path, fileName));
		return new FileContentResponse(content, GetMimeTypeFor(key), fileName);
	}

	private string GetFilenameFor(string key)
	{
		var name = key.ToLower() switch
		{
			"logo" => "logo.png",
			"index" => "index.html",
			"about" => "about.html",
			"beliefs" => "beliefs.html",
			"services" => "services.html",
			_ => null
		};

		if (name == null)
		{
			throw new InvalidContentFileKeyException(key);
		}
		return name;
	}

	private string GetMimeTypeFor(string key)
	{
		if (key.ToLower().Equals("logo"))
		{
			return "image/png";
		}
		return "text/html";
	}
}
