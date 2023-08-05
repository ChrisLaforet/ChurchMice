using System.Security.Authentication;
using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<LoginCommandHandler> logger;
	
	public LoginCommandHandler(IUserProxy userProxy, ILogger<LoginCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public LoginResponse Handle(LoginCommand command)
	{
		logger.Log(LogLevel.Information, "Request to log in " + command.Username);
		userProxy.ExpireUserTokens();

		try
		{
			var response = userProxy.AuthenticateUser(command.Username, command.Password);
			logger.Log(LogLevel.Information, "Successful login of user " + command.Username);
			return new LoginResponse(response);
		}
		catch (AuthenticationException ae)
		{
			logger.Log(LogLevel.Error, "Failure to log in " + command.Username);
			throw;
		}
	}
}