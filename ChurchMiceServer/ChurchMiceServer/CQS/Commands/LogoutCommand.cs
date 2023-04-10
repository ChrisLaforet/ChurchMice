namespace ChurchMiceServer.CQS.Commands;

public class LogoutCommand : ICommand
{
	public string Username
	{
		get;
	}

	public LogoutCommand(string username)
	{
		this.Username = username;
	}
}