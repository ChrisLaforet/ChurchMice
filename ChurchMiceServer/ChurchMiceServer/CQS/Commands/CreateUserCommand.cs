namespace ChurchMiceServer.CQS.Commands;

public class CreateUserCommand : ICommand
{
	public string Email { get; }

	public string UserName { get; }

	public string FullName { get; }


	public CreateUserCommand(string userName, string fullName, string email)
	{
		this.UserName = userName;
		this.Email = email;
		this.FullName = fullName;
	}
}