using ChurchMiceServer.Types;

namespace ChurchMiceServer.CQS.Commands;

public class SetUserPasswordCommand : ICommand
{
	public UserId UserId { get; }

	public string Password { get; }

	public SetUserPasswordCommand(UserId userId, string password)
	{
		this.UserId = userId;
		this.Password = password;
	}
}