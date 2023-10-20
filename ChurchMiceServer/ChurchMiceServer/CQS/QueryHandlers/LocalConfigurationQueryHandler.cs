using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.QueryHandlers;

public class LocalConfigurationQueryHandler : IQueryHandler<LocalConfigurationQuery, List<Tuple<string, string>>>
{
	private readonly IConfigurationProxy configurationProxy;
	private readonly ILogger<LocalConfigurationQueryHandler> logger;

	public LocalConfigurationQueryHandler(IConfigurationProxy configurationProxy, ILogger<LocalConfigurationQueryHandler> logger)
	{
		this.configurationProxy = configurationProxy;
		this.logger = logger;
	}
	
	public List<Tuple<string, string>> Handle(LocalConfigurationQuery query)
	{
		var response = new List<Tuple<string, string>>();
		try
		{
			var ministryName = configurationProxy.GetMinistryName();
			if (!string.IsNullOrEmpty(ministryName))
			{
				response.Add(new Tuple<string, string>(IConfigurationProxy.MINISTRY_NAME_KEYWORD, ministryName));
			}

			var baseUrl = configurationProxy.GetBaseUrl();
			if (!string.IsNullOrEmpty(baseUrl))
			{
				response.Add(new Tuple<string, string>(IConfigurationProxy.BASE_URL_KEYWORD, baseUrl));
			}
	
			var address1 = configurationProxy.GetMinistryAddress1();
			if (!string.IsNullOrEmpty(address1))
			{
				response.Add(new Tuple<string, string>(IConfigurationProxy.MINISTRY_ADDRESS1_KEYWORD, address1));
			}
			
			var address2 = configurationProxy.GetMinistryAddress2();
			if (!string.IsNullOrEmpty(address2))
			{
				response.Add(new Tuple<string, string>(IConfigurationProxy.MINISTRY_ADDRESS2_KEYWORD, address2));
			}
				
			var address3 = configurationProxy.GetMinistryAddress3();
			if (!string.IsNullOrEmpty(address3))
			{
				response.Add(new Tuple<string, string>(IConfigurationProxy.MINISTRY_ADDRESS3_KEYWORD, address3));
			}
			
			var phone = configurationProxy.GetMinistryPhone();
			if (!string.IsNullOrEmpty(phone))
			{
				response.Add(new Tuple<string, string>(IConfigurationProxy.MINISTRY_PHONE_KEYWORD, phone));
			}
	
			var facebookUrl = configurationProxy.GetFacebookUrl();
			if (!string.IsNullOrEmpty(facebookUrl))
			{
				response.Add(new Tuple<string, string>(IConfigurationProxy.FACEBOOK_URL_KEYWORD, facebookUrl));
			}
			
			var youTubeUrl = configurationProxy.GetYouTubeUrl();
			if (!string.IsNullOrEmpty(youTubeUrl))
			{
				response.Add(new Tuple<string, string>(IConfigurationProxy.YOUTUBE_URL_KEYWORD, youTubeUrl));
			}
			
			var vimeoUrl = configurationProxy.GetVimeoUrl();
			if (!string.IsNullOrEmpty(vimeoUrl))
			{
				response.Add(new Tuple<string, string>(IConfigurationProxy.VIMEO_URL_KEYWORD, vimeoUrl));
			}
			
			var instagramUrl = configurationProxy.GetInstagramUrl();
			if (!string.IsNullOrEmpty(instagramUrl))
			{
				response.Add(new Tuple<string, string>(IConfigurationProxy.INSTAGRAM_URL_KEYWORD, instagramUrl));
			}
		}
		catch (Exception e)
		{
			// ignore the exception
		}
		return response;
	}
}