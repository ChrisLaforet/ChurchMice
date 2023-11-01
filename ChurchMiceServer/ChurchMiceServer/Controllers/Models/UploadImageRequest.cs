namespace ChurchMiceServer.Controllers.Models
{
	public class UploadImageRequest
	{
		public string FileContentBase64 { get; set; }
		public int MemberId { get; set; }
		public string FileName { get; set; }
		public string FileType { get; set; }
		public long FileSize { get; set; }
	}
}
