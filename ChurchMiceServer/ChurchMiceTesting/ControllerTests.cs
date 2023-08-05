using ChurchMiceServer.Controllers;
using ChurchMiceServer.Controllers.Models;
using ChurchMiceServer.CQS.CommandHandlers;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Domains.Proxies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChurchMiceTesting;

public class ControllerTests
{
    private MockContext contextMock = new MockContext();
    private MockEmailProxy emailProxyMock = new MockEmailProxy();
    private MockConfigurationLoader configurationLoaderMock = new MockConfigurationLoader();
    private UserProxy userProxy;

    public ControllerTests()
    {
        userProxy = new UserProxy(contextMock.Get(), emailProxyMock.Get(), configurationLoaderMock.Get());
    }
    
    [Fact]
    public void GivenUserController_WhenLoggingInWithNonExistentUser_ThenFailsToAuthenticate()
    {
        var controller = CreateMockedUserController();
        var loggerMock = new Mock<ILogger<LoginCommandHandler>>();

        controller.LoginCommandHandler = new LoginCommandHandler(userProxy, loggerMock.Object);

        var request = new AuthenticateRequest()
        {
            Username = "test",
            Password = "test"
        };
        var result = controller.Login(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public void GivenUserController_WhenLoggingInWithExistingUserWithWrongPassword_ThenFailsToAuthenticate()
    {
        var controller = CreateMockedUserController();
        var loggerMock = new Mock<ILogger<LoginCommandHandler>>();

        controller.LoginCommandHandler = new LoginCommandHandler(userProxy, loggerMock.Object);
        userProxy.CreateUser(CreateTestUser());

        var request = new AuthenticateRequest()
        {
            Username = "test",
            Password = "test"
        };
        var result = controller.Login(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    private UserController CreateMockedUserController()
    {
        var loggerMock = new Mock<ILogger<UserController>>();
        return new UserController(null, userProxy, loggerMock.Object);
    }

    private static User CreateTestUser()
    {
        return new User()
        {
            Username = "test",
            PasswordHash = "ITgCz5ThWmm8COJxJIJVWCkYL13Ixb6Rwt4Z6i+iOe6pGbuOWnFMVxtfvsAZXaLK",
            Email = "test@test.com",
            Fullname = "test user"
        };
    }
}