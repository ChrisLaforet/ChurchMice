namespace ChurchMiceServer.Controllers.Models;

public class UpdateUserRequest
{
	public string UserId { get; set; }
	public string UserName { get; set; }
	public string Email { get; set; }
	public string FullName { get; set; }
}