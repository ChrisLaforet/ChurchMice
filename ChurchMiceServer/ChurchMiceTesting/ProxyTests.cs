using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Domains.Proxies;
using ChurchMiceTesting.Mocks;

namespace ChurchMiceTesting;

public class ProxyTests
{
    [Fact]
    public void GivenAUserProxy_WhenAddingAUser_ThenPersistsNewUser()
    {
        var contextMock = new MockRepositoryContext();
        var emailProxyMock = new MockEmailProxy();
        var configurationLoaderMock = new MockConfigurationLoader();
        var configurationProxyMock = new MockConfigurationProxy();

        var userProxy = new UserProxy(contextMock, emailProxyMock.Get(), configurationProxyMock.Get(), configurationLoaderMock.Get());
        var user = new User();
        user.Username = "username";
        user.Fullname = "full name";
        userProxy.CreateUser(user);
        Assert.Equal(1, contextMock.GetChangeCount());
    }

    [Fact]
    public void GivenAConfigurationProxy_WhenAddingKeywordValue_ThenPersistsKeyword()
    {
        var contextMock = new MockRepositoryContext();
        var proxyMock = new ConfigurationProxy(contextMock);
        proxyMock.SetUserContentPath("TestingPath");
        Assert.Equal(1, contextMock.GetChangeCount());
    }
}