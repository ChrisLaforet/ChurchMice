namespace ChurchMiceServer.CQS.Responses;

public class MemberImageResponse
{
	public int Id { get; private set; }
	public int MemberId { get; private set; }
	public string UploadUserId { get; private set; }
	public string FileContentBase64 { get; private set; }
	public string FileType { get; private set; }
	public bool IsApproved { get; private set; }
	public DateTime? UploadDate { private get; set; }

	public MemberImageResponse(int id, int memberId, string uploadUserId, string fileContentBase64,
								string fileType, bool isApproved, DateTime? uploadDate)
	{
		Id = id;
		MemberId = memberId;
		UploadUserId = uploadUserId;
		FileContentBase64 = fileContentBase64;
		FileType = fileType;
		IsApproved = isApproved;
		UploadDate = uploadDate;
	}
}