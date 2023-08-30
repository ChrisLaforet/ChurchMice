namespace ChurchMiceServer.CQS.Queries;

public class ContentFileQuery : IQuery
{
	public string Key { get; private set; }

	public ContentFileQuery(string key)
	{
		this.Key = key;
	}
}