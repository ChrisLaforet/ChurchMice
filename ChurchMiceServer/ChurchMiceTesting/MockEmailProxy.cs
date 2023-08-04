using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceTesting;

public class MockEmailProxy
{
    private Mock<IEmailProxy> emailProxy = new Mock<IEmailProxy>();

    public MockEmailProxy() {}

    public IEmailProxy Get()
    {
        return emailProxy.Object;
    }
    
    public void VerifySaveChanges(int expectedCount)
    {
        emailProxy.Verify(m => 
            m.SendMessageTo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), 
            Times.Exactly(expectedCount));
    }
}