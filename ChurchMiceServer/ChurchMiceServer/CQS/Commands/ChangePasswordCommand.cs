namespace ChurchMiceServer.CQS.Commands;

public class ChangePasswordCommand : ICommand
{
	public string UserName
	{
		get;
	}

	public ChangePasswordCommand(string userName)
	{
		this.UserName = userName;
	}
}