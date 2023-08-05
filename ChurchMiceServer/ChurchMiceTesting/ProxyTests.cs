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

        var userProxy = new UserProxy(contextMock, emailProxyMock.Get(), configurationLoaderMock.Get());
        var user = new User();
        user.Username = "username";
        user.Fullname = "full name";
        userProxy.CreateUser(user);
        Assert.Equal(1, contextMock.GetChangeCount());
    }
}