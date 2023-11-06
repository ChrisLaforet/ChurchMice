using ChurchMiceServer.Types;

namespace ChurchMiceServer.CQS.Commands
{
	public class UploadImageCommand : ICommand
	{
		public UserId UploadUserId { get; private set; }
		public string FileContentBase64 { get; set; }
		public string FileName { get; set; }
		public string FileType { get; set; }
		public long FileSize { get; set; }
		public MemberId MemberId { get; private set; }

		public UploadImageCommand(UserId uploadUserId, MemberId memberId, string fileContentBase64, string fileName, string fileType, long fileSize)
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
