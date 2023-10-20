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
			if (IsConfigurationDifferent(configurationProxy.GetMinistryName(), ministryName))
			{
				configurationProxy.SetMinistryName(ministryName);
				logger.LogInformation($"Changed ministry name to {ministryName}");
			}
		}
		
		if (!string.IsNullOrEmpty(command.BaseUrl))
		{
			var baseUrl = command.BaseUrl.Trim();
			if (IsConfigurationDifferent(configurationProxy.GetBaseUrl(), baseUrl) &&
			    IsUrlValid(baseUrl))
			{
				configurationProxy.SetBaseUrl(baseUrl);
				logger.LogInformation($"Changed base URL to {baseUrl}");
			}
		}

		if (!string.IsNullOrEmpty(command.MinistryAddress1))
		{
			var ministryAddress = command.MinistryAddress1.Trim();
			if (IsConfigurationDifferent(configurationProxy.GetMinistryAddress1(), ministryAddress))
			{
				configurationProxy.SetMinistryAddress1(ministryAddress);
				logger.LogInformation($"Changed ministry address 1 to {ministryAddress}");
			}
		}
		
		if (!string.IsNullOrEmpty(command.MinistryAddress2))
		{
			var ministryAddress = command.MinistryAddress2.Trim();
			if (IsConfigurationDifferent(configurationProxy.GetMinistryAddress2(), ministryAddress))
			{
				configurationProxy.SetMinistryAddress2(ministryAddress);
				logger.LogInformation($"Changed ministry address 2 to {ministryAddress}");
			}
		}
		
		if (!string.IsNullOrEmpty(command.MinistryAddress3))
		{
			var ministryAddress = command.MinistryAddress3.Trim();
			if (IsConfigurationDifferent(configurationProxy.GetMinistryAddress3(), ministryAddress))
			{
				configurationProxy.SetMinistryAddress3(ministryAddress);
				logger.LogInformation($"Changed ministry address 3 to {ministryAddress}");
			}
		}

		if (!string.IsNullOrEmpty(command.MinistryPhone))
		{
			var ministryPhone = command.MinistryPhone.Trim();
			if (IsConfigurationDifferent(configurationProxy.GetMinistryPhone(), ministryPhone))
			{
				configurationProxy.SetMinistryPhone(ministryPhone);
				logger.LogInformation($"Changed ministry phone to {ministryPhone}");
			}
		}
		
		if (!string.IsNullOrEmpty(command.FacebookUrl))
		{
			var url = command.FacebookUrl.Trim();
			if (IsConfigurationDifferent(configurationProxy.GetFacebookUrl(), url) &&
			    IsUrlValid(url))
			{
				configurationProxy.SetFacebookUrl(url);
				logger.LogInformation($"Changed facebook URL to {url}");
			}
		}
		
		if (!string.IsNullOrEmpty(command.YouTubeUrl))
		{
			var url = command.YouTubeUrl.Trim();
			if (IsConfigurationDifferent(configurationProxy.GetYouTubeUrl(), url) &&

			    IsUrlValid(url))
			{
				configurationProxy.SetYouTubeUrl(url);
				logger.LogInformation($"Changed youtube URL to {url}");
			}
		}
			
		if (!string.IsNullOrEmpty(command.VimeoUrl))
		{
			var url = command.VimeoUrl.Trim();
			if (IsConfigurationDifferent(configurationProxy.GetVimeoUrl(), url) &&
			    IsUrlValid(url))
			{
				configurationProxy.SetVimeoUrl(url);
				logger.LogInformation($"Changed vimeo URL to {url}");
			}
		}
			
		if (!string.IsNullOrEmpty(command.InstagramUrl))
		{
			var url = command.InstagramUrl.Trim();
			if (IsConfigurationDifferent(configurationProxy.GetInstagramUrl(), url) &&
			    IsUrlValid(url))
			{
				configurationProxy.SetInstagramUrl(url);
				logger.LogInformation($"Changed instagram URL to {url}");
			}
		}

		return new NothingnessResponse();
	}

	private bool IsConfigurationDifferent(string? newValue, string? oldValue)
	{
		if (newValue == null && oldValue == null)
		{
			return false;
		}

		if (newValue == null || oldValue == null)
		{
			return true;
		}

		return !string.Equals(newValue, oldValue);
	}

	private bool IsUrlValid(string url)
	{
		//https://stackoverflow.com/questions/7578857/how-to-check-whether-a-string-is-a-valid-http-url
		Uri uriResult;
		return Uri.TryCreate(url, UriKind.Absolute, out uriResult) 
		              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
	}
}