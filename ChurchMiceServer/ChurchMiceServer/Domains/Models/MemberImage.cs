namespace ChurchMiceServer.Domains.Models;

public partial class MemberImage
{
	public int Id { get; set; }
	public int MemberId { get; set; }
	public string Image { get; set; }
	public DateTime UploadDate { get; set; }
	public DateTime? ApproveDate { get; set; }
}