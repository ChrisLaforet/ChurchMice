namespace ChurchMiceServer.CQS.Commands;

public class NewUserCommand : ICommand
{
	public string Email
	{
		get;
	}

	public string UserName
	{
		get;
	}

	public string Password
	{
		get;
	}

	public string FullName
	{
		get;
	}

	public NewUserCommand(string userName, string password, string email, string fullName)
	{
		this.UserName = userName;
		this.Password = password;
		this.Email = email;
		this.FullName = fullName;
	}
}