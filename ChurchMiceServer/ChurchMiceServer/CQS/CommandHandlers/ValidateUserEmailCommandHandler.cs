using System.Security.Authentication;
using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class ValidateUserEmailCommandHandler : ICommandHandler<ValidateUserEmailCommand, NothingnessResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<ValidateUserEmailCommandHandler> logger;
	
	public ValidateUserEmailCommandHandler(IUserProxy userProxy, ILogger<ValidateUserEmailCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(ValidateUserEmailCommand command)
	{
		logger.LogInformation($"Request to validate Email address for {command.Username}");
		userProxy.ExpireUserTokens();

		try
		{
			userProxy.ValidateEmailForUser(command.Username, command.Password);
			logger.LogInformation($"Successful validation of user credentials for {command.Username} to validate Email address");
			return new NothingnessResponse();
		}
		catch (AuthenticationException ae)
		{
			logger.LogError($"Failure to log in {command.Username} in order to validate Email address");
			throw;
		}
	}
}