namespace ChurchMiceServer.CQS.Commands;

public class ChangePasswordCommand : ICommand
{
	public string Email
	{
		get;
	}

	public ChangePasswordCommand(string email)
	{
		this.Email = email;
	}
}