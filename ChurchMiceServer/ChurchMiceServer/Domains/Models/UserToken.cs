namespace ChurchMiceServer.Domains.Models;

public class UserToken
{
	public string Id { get; set; }
	public string UserId { get; set; }
	public string TokenKey { get; set; }
	public DateTime Created { get; set; }
	public DateTime Expired { get; set; }
}
