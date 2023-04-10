using System.Security.Authentication;
using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class SetPasswordCommandHandler : ICommandHandler<SetPasswordCommand, NothingnessResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<SetPasswordCommand> logger;
	
	public SetPasswordCommandHandler(IUserProxy userProxy, ILogger<SetPasswordCommand> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(SetPasswordCommand command)
	{
		logger.Log(LogLevel.Information, "Request to set password for " + command.Username);
		try
		{
			userProxy.SetPasswordFor(command.Username, command.ResetKey, command.Password);
			logger.Log(LogLevel.Information, "Successful password change for " + command.Username);
			return new NothingnessResponse();
		}
		catch (AuthenticationException ae)
		{
			logger.Log(LogLevel.Error, "Failure to set password for " + command.Username);
			throw;
		}
	}
}