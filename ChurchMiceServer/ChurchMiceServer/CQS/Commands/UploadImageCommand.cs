namespace ChurchMiceServer.CQS.Commands
{
	public class UploadImageCommand : ICommand
	{
		public string UploadUserId { get; private set; }
		public string Image { get; private set; }
		public int MemberId { get; private set; }

		public UploadImageCommand(string uploadUserId, int memberId, string image)
		{
			UploadUserId = uploadUserId;
			Image = image;
			MemberId = memberId;
		}
	}
}
