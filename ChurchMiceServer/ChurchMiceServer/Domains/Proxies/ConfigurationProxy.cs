using ChurchMiceServer.Domains.Exceptions;

namespace ChurchMiceServer.Domains.Proxies;

public class ConfigurationProxy : IConfigurationProxy
{
	private readonly IRepositoryContext context;

	public ConfigurationProxy(IRepositoryContext context)
	{
		this.context = context;
	}

	private void SetConfiguredValueFor(string key, string value)
	{
		var config = context.Configurations.Find(key);
		if (config != null)
		{
			config.Value = value;
		}
		else
		{
			context.Configurations.Add(new Models.Configuration() 
			{
				Keyword = key, 
				Value = value
			});
		}
		context.SaveChanges();
	}
	
	public void SetUserContentPath(string path)
	{
		SetConfiguredValueFor(IConfigurationProxy.CONTENT_PATH_KEYWORD, path);
	}

	private string GetConfiguredValueFor(string key)
	{
		var contentPath = context.Configurations.Find(key);
		if (contentPath != null)
		{
			return contentPath.Value;
		}
		throw new NotConfiguredException(key);
	}
	
	public string GetUserContentPath()
	{
		return GetConfiguredValueFor(IConfigurationProxy.CONTENT_PATH_KEYWORD);
	}

	public void SetChurchName(string name)
	{
		SetConfiguredValueFor(IConfigurationProxy.CHURCH_NAME_KEYWORD, name);
	}

	public string GetChurchName()
	{
		return GetConfiguredValueFor(IConfigurationProxy.CHURCH_NAME_KEYWORD);
	}
}