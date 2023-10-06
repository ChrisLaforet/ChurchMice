namespace ChurchMiceServer.CQS.Commands;

public class SetUserRoleCommand : ICommand
{
	public string UserId { get; }
	public string RoleLevelCode { get; }

	public SetUserRoleCommand(string userId, string roleLevelCode)
	{
		this.UserId = userId;
		this.RoleLevelCode = roleLevelCode;
	}
}