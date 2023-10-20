namespace ChurchMiceServer.Controllers.Models;

public class LocalConfigurationChangeRequest
{
	public string MinistryName { get; set; }
	public string BaseUrl { get; set; }
	public string? MinistryAddress1 { get; set; }
	public string? MinistryAddress2 { get; set; }
	public string? MinistryAddress3 { get; set; }
	public string? MinistryPhone { get; set; }
	public string? FacebookUrl { get; set; }
	public string? YouTubeUrl { get; set; }
	public string? VimeoUrl { get; set; }
	public string? InstagramUrl { get; set; }
}