using ChurchMiceServer.Domains.Exceptions;

namespace ChurchMiceServer.Domains.Proxies;

public class ConfigurationProxy : IConfigurationProxy
{
	private readonly IRepositoryContext context;

	public ConfigurationProxy(IRepositoryContext context)
	{
		this.context = context;
	}
	
	public void SetUserContentPath(string path)
	{
		var contentPath = context.Configurations.Find(IConfigurationProxy.CONTENT_PATH_KEYWORD);
		if (contentPath != null)
		{
			contentPath.Value = path;
		}
		else
		{
			context.Configurations.Add(new Models.Configuration() 
				{
					Keyword = IConfigurationProxy.CONTENT_PATH_KEYWORD, 
					Value = path
				});
		}
		context.SaveChanges();
	}

	public string GetUserContentPath()
	{
		var contentPath = context.Configurations.Find(IConfigurationProxy.CONTENT_PATH_KEYWORD);
		if (contentPath != null)
		{
			return contentPath.Value;
		}
		throw new NotConfiguredException(IConfigurationProxy.CONTENT_PATH_KEYWORD);
	}
}