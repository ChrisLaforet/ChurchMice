namespace ChurchMiceServer.CQS.Commands;

public class SetLocalConfigurationCommand : ICommand
{
	public string MinistryName { get;}
	public string BaseUrl { get; }

	public SetLocalConfigurationCommand(string ministryName, string baseUrl)
	{
		this.MinistryName = ministryName;
		this.BaseUrl = baseUrl;
	}
}