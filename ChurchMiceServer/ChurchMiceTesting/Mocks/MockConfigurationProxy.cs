using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceTesting.Mocks;

public class MockConfigurationProxy
{
	private Mock<IConfigurationProxy> configurationProxy = new Mock<IConfigurationProxy>();

	public MockConfigurationProxy() {}
	
	public IConfigurationProxy Get()
	{
		return configurationProxy.Object;
	}
    
	public void VerifySaveChanges(int expectedCount)
	{
		configurationProxy.Verify(m => 
				m.SetUserContentPath(It.IsAny<string>()), 
			Times.Exactly(expectedCount));
	}
}