namespace ChurchMiceServer.Domains.Models;

public partial class User
{
	public String Id { get; set; }

	public Guid Guid
	{
		get
		{
			return new Guid(Id);
		}
		set
		{
			Id = value.ToString();
		}
	}

	public String Username { get; set; }
	public String Email { get; set; }
	public DateTime CreateDate { get; set; }
	public String? PasswordHash { get; set; }
	public DateTime? LastLoginDatetime { get; set; }
	public String? ResetKey { get; set; }
	public DateTime? ResetExpirationDatetime { get; set; }

}