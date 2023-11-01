namespace ChurchMiceServer.CQS.Commands
{
	public class UploadImageCommand : ICommand
	{
		public string UploadUserId { get; private set; }
		public string FileContentBase64 { get; set; }
		public string FileName { get; set; }
		public string FileType { get; set; }
		public long FileSize { get; set; }
		public int MemberId { get; private set; }

		public UploadImageCommand(string uploadUserId, int memberId, string fileContentBase64, string fileName, string fileType, long fileSize)
		{
			UploadUserId = uploadUserId;
			MemberId = memberId;
			FileContentBase64 = fileContentBase64;
			FileName = fileName;
			FileType = fileType;
			FileSize = fileSize;
		}
	}
}
