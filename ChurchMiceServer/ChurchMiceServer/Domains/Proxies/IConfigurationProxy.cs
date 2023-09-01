namespace ChurchMiceServer.Domains.Proxies;

public interface IConfigurationProxy
{
	public const string CONTENT_PATH_KEYWORD = "ContentPath";
	public const string MINISTRY_NAME_KEYWORD = "MinistryName";
	
	void SetUserContentPath(string path);
	string GetUserContentPath();

	void SetMinistryName(string name);
	string GetMinistryName();
}