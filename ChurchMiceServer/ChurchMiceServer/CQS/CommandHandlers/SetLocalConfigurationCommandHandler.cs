using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.QueryHandlers;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class SetLocalConfigurationCommandHandler : ICommandHandler<SetLocalConfigurationCommand, NothingnessResponse>
{
	private readonly IConfigurationProxy configurationProxy;
	private readonly ILogger<SetLocalConfigurationCommandHandler> logger;

	public SetLocalConfigurationCommandHandler(IConfigurationProxy configurationProxy, ILogger<SetLocalConfigurationCommandHandler> logger)
	{
		this.configurationProxy = configurationProxy;
		this.logger = logger;
	}

	public NothingnessResponse Handle(SetLocalConfigurationCommand command)
	{
		if (!string.IsNullOrEmpty(command.MinistryName))
		{
			var ministryName = command.MinistryName.Trim();
			if (configurationProxy.GetMinistryName() != null &&
			    !ministryName.Equals(configurationProxy.GetMinistryName()))
			{
				configurationProxy.SetMinistryName(ministryName);
				logger.LogInformation($"Changed ministry name to {ministryName}");
			}
		}
		
		if (!string.IsNullOrEmpty(command.BaseUrl))
		{
			var baseUrl = command.BaseUrl.Trim();
			if (configurationProxy.GetBaseUrl() != null &&
			    !baseUrl.Equals(configurationProxy.GetBaseUrl()) &&
			    IsUrlValid(baseUrl))
			{
				configurationProxy.SetBaseUrl(baseUrl);
				logger.LogInformation($"Changed base URL to {baseUrl}");
			}
		}

		return new NothingnessResponse();
	}

	private bool IsUrlValid(string url)
	{
		//https://stackoverflow.com/questions/7578857/how-to-check-whether-a-string-is-a-valid-http-url
		Uri uriResult;
		return Uri.TryCreate(url, UriKind.Absolute, out uriResult) 
		              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
	}
}