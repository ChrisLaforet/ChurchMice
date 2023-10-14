namespace ChurchMiceServer.Domains.Models;

public partial class MemberEditor
{
	public int Id { get; set; }
	public int MemberId { get; set; }
	public string EditorId { get; set; }
}