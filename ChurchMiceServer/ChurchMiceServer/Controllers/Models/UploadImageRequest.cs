namespace ChurchMiceServer.Controllers.Models
{
	public class UploadImageRequest
	{
		public string UploadUserId { get; set; }
		public string Image { get; set; }
		public int MemberId { get; set; }
	}
}
