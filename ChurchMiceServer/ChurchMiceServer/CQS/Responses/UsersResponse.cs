namespace ChurchMiceServer.CQS.Responses;

public class UsersResponse
{
	public List<UserResponse> Users { get; }

	public UsersResponse(List<UserResponse> users)
	{
		this.Users = users;
	}
}