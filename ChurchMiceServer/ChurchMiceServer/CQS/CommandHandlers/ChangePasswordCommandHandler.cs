using System.Security.Authentication;
using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, NothingnessResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<ChangePasswordCommandHandler> logger;
	
	public ChangePasswordCommandHandler(IUserProxy userProxy, ILogger<ChangePasswordCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(ChangePasswordCommand command)
	{
		logger.Log(LogLevel.Information, "Request to change password for " + command.Email);
		try
		{
			userProxy.ChangePasswordFor(command.Email);
		}
		catch (Exception ex)
		{
			logger.Log(LogLevel.Error, "Caught an exception during request to change password: " + ex);
		}
		return new NothingnessResponse();
	}
}