namespace ChurchMiceServer.Configuration
{

	public interface IConfigurationLoader
	{
		string GetKeyValueFor(string elementName);
	}
}
