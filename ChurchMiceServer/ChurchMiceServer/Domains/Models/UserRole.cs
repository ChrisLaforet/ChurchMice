namespace ChurchMiceServer.Domains.Models;

public partial class UserRole
{
	public int Id { get; set; }
	public String UserId { get; set; }
	public int RoleLevel { get; set; }

	public virtual User User { get; set; }
}