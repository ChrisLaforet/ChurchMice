namespace ChurchMiceServer.Controllers.Models;

public class CreateUserRequest
{
	public string UserName { get; set; }
	public string Email { get; set; }
	public string FullName { get; set; }
}