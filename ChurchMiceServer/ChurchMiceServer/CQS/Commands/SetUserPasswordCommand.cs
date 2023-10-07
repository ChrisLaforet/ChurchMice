namespace ChurchMiceServer.CQS.Commands;

public class SetUserPasswordCommand : ICommand
{
	public string UserId { get; }

	public string Password { get; }

	public SetUserPasswordCommand(string userId, string password)
	{
		this.UserId = userId;
		this.Password = password;
	}
}