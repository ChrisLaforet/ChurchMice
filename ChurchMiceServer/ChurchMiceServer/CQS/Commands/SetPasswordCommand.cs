namespace ChurchMiceServer.CQS.Commands;

public class SetPasswordCommand : ICommand
{
	public string Username
	{
		get;
	}

	public string ResetKey
	{
		get;
	}

	public string Password
	{
		get;
	}

	public SetPasswordCommand(string username, string resetKey, string password)
	{
		this.Username = username;
		this.ResetKey = resetKey;
		this.Password = password;
	}
}