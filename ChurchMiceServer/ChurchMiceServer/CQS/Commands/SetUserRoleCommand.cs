using ChurchMiceServer.Types;

namespace ChurchMiceServer.CQS.Commands;

public class SetUserRoleCommand : ICommand
{
	public UserId UserId { get; }
	public RoleLevelCode RoleLevelCode { get; }

	public SetUserRoleCommand(UserId userId, RoleLevelCode roleLevelCode)
	{
		this.UserId = userId;
		this.RoleLevelCode = roleLevelCode;
	}
}