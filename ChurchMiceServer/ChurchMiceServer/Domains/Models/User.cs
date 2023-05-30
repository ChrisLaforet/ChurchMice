using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchMiceServer.Domains.Models;

public partial class User
{
	public string Id { get; set; }
	public string Username { get; set; }
	public string Fullname { get; set; }
	public string Email { get; set; }
	public DateTime CreateDate { get; set; }
	public string? PasswordHash { get; set; }
	public DateTime? LastLoginDatetime { get; set; }
	public string? ResetKey { get; set; }
	public DateTime? ResetExpirationDatetime { get; set; }

	[NotMapped]
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
}