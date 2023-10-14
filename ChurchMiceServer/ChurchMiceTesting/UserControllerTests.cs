using ChurchMiceServer.Controllers;
using ChurchMiceServer.Controllers.Models;
using ChurchMiceServer.CQS.CommandHandlers;
using ChurchMiceServer.CQS.QueryHandlers;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Domains.Proxies;
using ChurchMiceTesting.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChurchMiceTesting;

public class UserControllerTests
{
    private MockRepositoryContext contextMock = new MockRepositoryContext();
    private MockEmailProxy emailProxyMock = new MockEmailProxy();
    private MockConfigurationLoader configurationLoaderMock = new MockConfigurationLoader(); 
    private MockConfigurationProxy configurationProxyMock = new MockConfigurationProxy();

    private Mock<ILogger<LoginCommandHandler>> loginCommandLoggerMock = new Mock<ILogger<LoginCommandHandler>>();
    private Mock<ILogger<CheckExistingNameQueryHandler>> checkExistingNameCommandHandlerLoggerMock = new Mock<ILogger<CheckExistingNameQueryHandler>>();

    private UserProxy userProxy;
    private UserController userController;

    public UserControllerTests()
    {
        userProxy = new UserProxy(contextMock, emailProxyMock.Get(), configurationProxyMock.Get(), configurationLoaderMock.Get());
        userController = CreateMockedUserController();
    }
    
    [Fact]
    public void GivenUserController_WhenLoggingInWithNonExistentUser_ThenFailsToAuthenticate()
    {
        userController.LoginCommandHandler = new LoginCommandHandler(userProxy, loginCommandLoggerMock.Object);

        var request = new AuthenticateRequest()
        {
            Username = "test",
            Password = "test"
        };
        var result = userController.Login(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public void GivenUserController_WhenLoggingInWithExistingUserWithWrongPassword_ThenFailsToAuthenticate()
    {
        userController.LoginCommandHandler = new LoginCommandHandler(userProxy, loginCommandLoggerMock.Object);
        userProxy.CreateUser(CreateTestUser());

        var request = new AuthenticateRequest()
        {
            Username = "test",
            Password = "test"
        };
        var result = userController.Login(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void GivenUserController_WhenLoggingInWithExistingUserWithCorrectPassword_ThenAuthenticatesSuccessfully()
    {
        userController.LoginCommandHandler = new LoginCommandHandler(userProxy, loginCommandLoggerMock.Object);
        userProxy.CreateUser(CreateTestUser());

        var request = new AuthenticateRequest()
        {
            Username = "test",
            Password = "Password"
        };
        var result = userController.Login(request);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void GivenUserController_WhenCheckingForExistingName_ThenReturnsBadRequest()
    {
        userController.CheckExistingNameQueryHandler = new CheckExistingNameQueryHandler(userProxy, checkExistingNameCommandHandlerLoggerMock.Object);
        userProxy.CreateUser(CreateTestUser());

        var request = new CheckUserNameAvailableRequest()
        {
            UserName = "test"
        };
        var result = userController.CheckUserNameAvailable(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    // [Fact]
    // public void GivenUserController_WhenCheckingForExistingEmail_ThenReturnsBadRequest()
    // {
    //     userController.CheckExistingNameQueryHandler = new CheckExistingNameQueryHandler(userProxy, checkExistingNameCommandHandlerLoggerMock.Object);
    //     userProxy.CreateUser(CreateTestUser());
    //
    //     var request = new CheckUserNameAvailableRequest()
    //     {
    //         CheckField = "Email",
    //         CheckValue = "test@test.com"
    //     };
    //     var result = userController.CheckExistingName(request);
    //
    //     Assert.IsType<BadRequestObjectResult>(result);
    // }

    [Fact]
    public void GivenUserController_WhenCheckingForNonExistantName_ThenReturnsOk()
    {
        userController.CheckExistingNameQueryHandler = new CheckExistingNameQueryHandler(userProxy, checkExistingNameCommandHandlerLoggerMock.Object);
        userProxy.CreateUser(CreateTestUser());

        var request = new CheckUserNameAvailableRequest()
        {
            UserName = "nobody"
        };
        var result = userController.CheckUserNameAvailable(request);

        Assert.IsType<OkObjectResult>(result);
    }
    
    // [Fact]
    // public void GivenUserController_WhenCheckingForNonExistantEmail_ThenReturnsOk()
    // {
    //     userController.CheckExistingNameQueryHandler = new CheckExistingNameQueryHandler(userProxy, checkExistingNameCommandHandlerLoggerMock.Object);
    //     userProxy.CreateUser(CreateTestUser());
    //
    //     var request = new CheckUserNameAvailableRequest()
    //     {
    //         CheckField = "Email",
    //         CheckValue = "nobody@test.com"
    //     };
    //     var result = userController.CheckExistingName(request);
    //
    //     Assert.IsType<OkObjectResult>(result);
    // }
    
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