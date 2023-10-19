namespace ChurchMiceServer.Domains.Proxies;

public interface IConfigurationProxy
{
	public const string CONTENT_PATH_KEYWORD = "ContentPath";
	
	public const string MINISTRY_NAME_KEYWORD = "MinistryName";
	public const string MINISTRY_ADDRESS1_KEYWORD = "MinistryAddress1";
	public const string MINISTRY_ADDRESS2_KEYWORD = "MinistryAddress2";
	public const string MINISTRY_ADDRESS3_KEYWORD = "MinistryAddress3";
	public const string MINISTRY_PHONE_KEYWORD = "MinistryPhone";
	
	public const string BASE_URL_KEYWORD = "BaseUrl";
	
	public const string FACEBOOK_URL_KEYWORD = "FacebookUrl";
	public const string YOUTUBE_URL_KEYWORD = "YouTubeUrl";
	public const string VIMEO_URL_KEYWORD = "VimeoUrl";
	public const string INSTAGRAM_URL_KEYWORD = "InstagramUrl";
    
	void SetUserContentPath(string path);
	string GetUserContentPath();

	void SetMinistryName(string name);
	string GetMinistryName();
	
	void SetMinistryAddress1(string address);
	string GetMinistryAddress1();
	
	void SetMinistryAddress2(string address);
	string GetMinistryAddress2();
	
	void SetMinistryAddress3(string address);
	string GetMinistryAddress3();

	void SetMinistryPhone(string phone);
	string GetMinistryPhone();
	
	void SetBaseUrl(string url);
	string GetBaseUrl();
	
	void SetFacebookUrl(string url);
	string GetFacebookUrl();
	
	void SetYouTubeUrl(string url);
	string GetYouTubeUrl();

	void SetVimeoUrl(string url);
	string GetVimeoUrl();

	void SetInstagramUrl(string url);
	string GetInstagramUrl();
}