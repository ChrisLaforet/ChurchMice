namespace ChurchMiceServer.Domains.Proxies;

public interface IConfigurationProxy
{
	const string CONTENT_PATH_KEYWORD = "ContentPath";
	
	void SetUserContentPath(string path);
	string GetUserContentPath();
}