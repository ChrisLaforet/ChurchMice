using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.Controllers.Models;

public class ConfigurationValuesResponse
{
	public string? MinistryName { get; set; }
	public string? BaseUrl { get; set; }
	public string? MinistryAddress1 { get; set; }
	public string? MinistryAddress2 { get; set; }
	public string? MinistryAddress3 { get; set; }
	public string? MinistryPhone { get; set; }
	public string? FacebookUrl { get; set; }
	public string? YouTubeUrl { get; set; }
	public string? VimeoUrl { get; set; }
	public string? InstagramUrl { get; set; }

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
			else if (pair.Item1.Equals(IConfigurationProxy.MINISTRY_ADDRESS1_KEYWORD))
			{
				this.MinistryAddress1 = pair.Item2;
			}			
			else if (pair.Item1.Equals(IConfigurationProxy.MINISTRY_ADDRESS2_KEYWORD))
			{
				this.MinistryAddress2 = pair.Item2;
			}
			else if (pair.Item1.Equals(IConfigurationProxy.MINISTRY_ADDRESS3_KEYWORD))
			{
				this.MinistryAddress3 = pair.Item2;
			}	
			else if (pair.Item1.Equals(IConfigurationProxy.MINISTRY_PHONE_KEYWORD))
			{
				this.MinistryPhone = pair.Item2;
			}	
			else if (pair.Item1.Equals(IConfigurationProxy.FACEBOOK_URL_KEYWORD))
			{
				this.FacebookUrl = pair.Item2;
			}
			else if (pair.Item1.Equals(IConfigurationProxy.YOUTUBE_URL_KEYWORD))
			{
				this.YouTubeUrl = pair.Item2;
			}
			else if (pair.Item1.Equals(IConfigurationProxy.VIMEO_URL_KEYWORD))
			{
				this.VimeoUrl = pair.Item2;
			}
			else if (pair.Item1.Equals(IConfigurationProxy.INSTAGRAM_URL_KEYWORD))
			{
				this.InstagramUrl = pair.Item2;
			}
		}
	}
}