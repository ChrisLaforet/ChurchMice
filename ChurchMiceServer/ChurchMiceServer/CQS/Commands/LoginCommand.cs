namespace ChurchMiceServer.CQS.Commands;

public class LoginCommand : ICommand
{
	public string Username
	{
		get;
	}

	public string Password
	{
		get;
	}

	public LoginCommand(string username, string password)
	{
		this.Username = username;
		this.Password = password;
	}
}