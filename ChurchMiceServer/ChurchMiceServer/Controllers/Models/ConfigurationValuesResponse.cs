using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.Controllers.Models;

public class ConfigurationValuesResponse
{
	public string? MinistryName { get; set; }
	public string? BaseUrl { get; set; }

	public ConfigurationValuesResponse(List<Tuple<string, string>> pairs)
	{
		foreach (var pair in pairs)
		{
			if (pair.Item1.Equals(IConfigurationProxy.MINISTRY_NAME_KEYWORD))
			{
				this.MinistryName = pair.Item2;
			}
			else if (pair.Item1.Equals(IConfigurationProxy.BASE_URL_KEYWORD))
			{
				this.BaseUrl = pair.Item2;
			}
		}
	}
}