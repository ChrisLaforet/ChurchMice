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
			
			
			
		}
		catch (Exception e)
		{
			// ignore the exception
		}
		return response;
	}
}