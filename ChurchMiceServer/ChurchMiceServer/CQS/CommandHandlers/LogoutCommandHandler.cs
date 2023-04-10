using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand, NothingnessResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<LogoutCommandHandler> logger;

	public LogoutCommandHandler(IUserProxy userProxy, ILogger<LogoutCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(LogoutCommand command)
	{
		try
		{
			logger.Log(LogLevel.Information, "Request to log out " + command.Username);
			userProxy.LogoutUser(command.Username);
		}
		catch (Exception ex)
		{
			logger.Log(LogLevel.Debug, "Error while logging out " + command.Username + ": " + ex);
		}

		return new NothingnessResponse();
	}
}