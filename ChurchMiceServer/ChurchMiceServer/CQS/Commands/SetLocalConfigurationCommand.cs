namespace ChurchMiceServer.CQS.Commands;

public class SetLocalConfigurationCommand : ICommand
{
	public string MinistryName { get;}
	public string BaseUrl { get; }
	public string? MinistryAddress1 { get; }
	public string? MinistryAddress2 { get; }
	public string? MinistryAddress3 { get; }
	public string? MinistryPhone { get; }
	public string? FacebookUrl { get; }
	public string? YouTubeUrl { get; }
	public string? VimeoUrl { get; }
	public string? InstagramUrl { get; }

	public SetLocalConfigurationCommand(string ministryName, string baseUrl, string? ministryAddress1,
		string? ministryAddress2, string? ministryAddress3, string? ministryPhone, string? facebookUrl,
		string? youTubeUrl, string? vimeoUrl, string? instagramUrl)
	{
		this.MinistryName = ministryName;
		this.BaseUrl = baseUrl;
		this.MinistryAddress1 = ministryAddress1;
		this.MinistryAddress2 = ministryAddress2;
		this.MinistryAddress3 = ministryAddress3;
		this.MinistryPhone = ministryPhone;
		this.FacebookUrl = facebookUrl;
		this.YouTubeUrl = youTubeUrl;
		this.VimeoUrl = vimeoUrl;
		this.InstagramUrl = instagramUrl;
	}
}