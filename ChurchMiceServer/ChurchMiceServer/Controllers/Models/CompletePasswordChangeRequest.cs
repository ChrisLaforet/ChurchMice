using System.ComponentModel.DataAnnotations;

namespace ChurchMiceServer.Controllers.Models;

public class CompletePasswordChangeRequest
{
	[Required]
	public string Username { get; set; }

	[Required]
	public string ResetKey { get; set; }

	[Required]
	public string Password { get; set; }
}