namespace ChurchMiceServer.Controllers.Models
{
	public class UploadImageRequest
	{
		public byte[] FileContent { get; set; }
		public int MemberId { get; set; }
	}
}
