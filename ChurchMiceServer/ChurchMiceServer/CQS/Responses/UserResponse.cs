using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Types;

namespace ChurchMiceServer.CQS.Responses;

public class UserResponse
{
	public string Id { get; }
	public string UserName { get; }
	public string FullName { get; }
	public string Email { get; }
	public string RoleLevel { get; }
	public string CreatedDate { get; }
	public string? LastLoginDate { get; }
    
	public UserResponse(User user, RoleLevelCode roleLevelCode)
	{
		this.Id = user.Id;
		this.UserName = user.Username;
		this.FullName = user.Fullname;
		this.Email = user.Email;
		this.RoleLevel = roleLevelCode.Code;
		this.CreatedDate = user.CreateDate.ToString();
		if (user.LastLoginDatetime != null)
		{
			this.LastLoginDate = user.LastLoginDatetime.ToString();
		}
	}
}