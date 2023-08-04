using ChurchMiceServer;
using ChurchMiceServer.Configuration;
using ChurchMiceServer.Services;

namespace ChurchMiceTesting;

public class MockConfigurationLoader
{
    private Mock<IConfigurationLoader> configurationLoader = new Mock<IConfigurationLoader>();

    public MockConfigurationLoader()
    {
        configurationLoader.Setup(m => m.GetKeyValueFor(Startup.SALT_STRING_KEY)).Returns("QVGppHXsmfYz5scu+RQ60g==");
        configurationLoader.Setup(m => m.GetKeyValueFor(Startup.NONCE_STRING_KEY)).Returns("98HjGFYb3- 89d73b6$$h +287hfkbj23fFobhoih=");
        configurationLoader.Setup(m => m.GetKeyValueFor(Startup.KEYITERATIONS_VALUE_KEY)).Returns("512");

        configurationLoader.Setup(m => m.GetKeyValueFor(IEmailSenderService.SMTP_SENDER)).Returns("test@test.com");
    }
    
    public IConfigurationLoader Get()
    {
        return configurationLoader.Object;
    }
}