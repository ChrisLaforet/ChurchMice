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
			if (ministryName.Length > 0)
			{
				response.Add(new Tuple<string, string>("MinistryName", ministryName));
			}
		}
		catch (Exception e)
		{
			// ignore the exception
		}
		return response;
	}
}