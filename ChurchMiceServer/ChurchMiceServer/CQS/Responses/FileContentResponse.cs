namespace ChurchMiceServer.CQS.Responses;

public class FileContentResponse
{
	public byte[] Content { get; private set; }
	public string MimeType { get; private set; }
	public string FileName { get; private set; }

	public FileContentResponse(byte[] content, string mimeType, string fileName)
	{
		this.Content = content;
		this.MimeType = mimeType;
		this.FileName = fileName;
	}
}