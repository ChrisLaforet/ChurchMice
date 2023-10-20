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

	public void SetMinistryName(string name)
	{
		SetConfiguredValueFor(IConfigurationProxy.MINISTRY_NAME_KEYWORD, name);
	}

	public string GetMinistryName()
	{
		try
		{
			return GetConfiguredValueFor(IConfigurationProxy.MINISTRY_NAME_KEYWORD);
		}
		catch (NotConfiguredException)
		{
			return null;
		}
	}

	public void SetBaseUrl(string url)
	{
		SetConfiguredValueFor(IConfigurationProxy.BASE_URL_KEYWORD, url);
	}

	public string GetBaseUrl()
	{
		try
		{
			return GetConfiguredValueFor(IConfigurationProxy.BASE_URL_KEYWORD);
		}
		catch (NotConfiguredException)
		{
			return null;
		}
	}
	
	public void SetMinistryAddress1(string address)
	{
		SetConfiguredValueFor(IConfigurationProxy.MINISTRY_ADDRESS1_KEYWORD, address);
	}

	public string GetMinistryAddress1()
	{
		try
		{
			return GetConfiguredValueFor(IConfigurationProxy.MINISTRY_ADDRESS1_KEYWORD);
		}
		catch (NotConfiguredException)
		{
			return null;
		}
	}
		
	public void SetMinistryAddress2(string address)
	{
		SetConfiguredValueFor(IConfigurationProxy.MINISTRY_ADDRESS2_KEYWORD, address);
	}

	public string GetMinistryAddress2()
	{
		try
		{
			return GetConfiguredValueFor(IConfigurationProxy.MINISTRY_ADDRESS2_KEYWORD);
		}
		catch (NotConfiguredException)
		{
			return null;
		}
	}
		
	public void SetMinistryAddress3(string address)
	{
		SetConfiguredValueFor(IConfigurationProxy.MINISTRY_ADDRESS3_KEYWORD, address);
	}

	public string GetMinistryAddress3()
	{
		try
		{
			return GetConfiguredValueFor(IConfigurationProxy.MINISTRY_ADDRESS3_KEYWORD);
		}
		catch (NotConfiguredException)
		{
			return null;
		}
	}
		
	public void SetMinistryPhone(string phone)
	{
		SetConfiguredValueFor(IConfigurationProxy.MINISTRY_PHONE_KEYWORD, phone);
	}

	public string GetMinistryPhone()
	{
		try
		{
			return GetConfiguredValueFor(IConfigurationProxy.MINISTRY_PHONE_KEYWORD);
		}
		catch (NotConfiguredException)
		{
			return null;
		}
	}
	
	public void SetFacebookUrl(string url)
	{
		SetConfiguredValueFor(IConfigurationProxy.FACEBOOK_URL_KEYWORD, url);
	}

	public string GetFacebookUrl()
	{
		try
		{
			return GetConfiguredValueFor(IConfigurationProxy.FACEBOOK_URL_KEYWORD);
		}
		catch (NotConfiguredException)
		{
			return null;
		}
	}
	
	public void SetYouTubeUrl(string url)
	{
		SetConfiguredValueFor(IConfigurationProxy.YOUTUBE_URL_KEYWORD, url);
	}

	public string GetYouTubeUrl()
	{
		try
		{
			return GetConfiguredValueFor(IConfigurationProxy.YOUTUBE_URL_KEYWORD);
		}
		catch (NotConfiguredException)
		{
			return null;
		}
	}
	
	public void SetVimeoUrl(string url)
	{
		SetConfiguredValueFor(IConfigurationProxy.VIMEO_URL_KEYWORD, url);
	}

	public string GetVimeoUrl()
	{
		try
		{
			return GetConfiguredValueFor(IConfigurationProxy.VIMEO_URL_KEYWORD);
		}
		catch (NotConfiguredException)
		{
			return null;
		}
	}
	
	public void SetInstagramUrl(string url)
	{
		SetConfiguredValueFor(IConfigurationProxy.INSTAGRAM_URL_KEYWORD, url);
	}

	public string GetInstagramUrl()
	{
		try
		{
			return GetConfiguredValueFor(IConfigurationProxy.INSTAGRAM_URL_KEYWORD);
		}
		catch (NotConfiguredException)
		{
			return null;
		}
	}
}