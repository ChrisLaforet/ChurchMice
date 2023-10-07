using System.Security.Authentication;
using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class SetUserPasswordCommandHandler : ICommandHandler<SetUserPasswordCommand, NothingnessResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<SetUserPasswordCommandHandler> logger;
	
	public SetUserPasswordCommandHandler(IUserProxy userProxy, ILogger<SetUserPasswordCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(SetUserPasswordCommand command)
	{
		logger.LogInformation($"Request to set password for user with Id of {command.UserId}");

		try
		{
			userProxy.SetPasswordFor(command.UserId, command.Password);
			logger.LogInformation($"Successful password change for user with Id of {command.UserId}");
			return new NothingnessResponse();
		}
		catch (AuthenticationException ae)
		{
			logger.LogError($"Failure to set password for user with Id of {command.UserId}");
			throw;
		}
	}
}