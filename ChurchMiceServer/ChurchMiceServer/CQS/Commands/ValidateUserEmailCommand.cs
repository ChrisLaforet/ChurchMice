namespace ChurchMiceServer.CQS.Commands;

public class ValidateUserEmailCommand : ICommand
{
	public string Username
	{
		get;
	}

	public string Password
	{
		get;
	}

	public ValidateUserEmailCommand(string username, string password)
	{
		this.Username = username;
		this.Password = password;
	}
}