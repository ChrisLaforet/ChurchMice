using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Domains.Proxies;
using Microsoft.EntityFrameworkCore;

namespace ChurchMiceTesting;

public class UnitTests
{
    [Fact]
    public void GivenAUserProxy_WhenAddingAUser_ThenPersistsAUser()
    {
        var contextMock = new MockContext();
        var emailProxyMock = new MockEmailProxy();
        var configurationLoaderMock = new MockConfigurationLoader();

        var userProxy = new UserProxy(contextMock.Get(), emailProxyMock.Get(), configurationLoaderMock.Get());
        var user = new User();
        user.Username = "username";
        user.Fullname = "full name";
        userProxy.CreateUser(user);
        contextMock.VerifySaveChanges(1);
    }
}