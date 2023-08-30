namespace ChurchMiceServer.Configuration
{
	// System configuration, not user configuration

	public interface IConfigurationLoader
	{
		string GetKeyValueFor(string elementName);
	}
}
