namespace ChurchMiceServer.Domains.Proxies;

public interface IConfigurationProxy
{
	public const string CONTENT_PATH_KEYWORD = "ContentPath";
	public const string CHURCH_NAME_KEYWORD = "ChurchName";
	
	void SetUserContentPath(string path);
	string GetUserContentPath();

	void SetChurchName(string name);
	string GetChurchName();
}