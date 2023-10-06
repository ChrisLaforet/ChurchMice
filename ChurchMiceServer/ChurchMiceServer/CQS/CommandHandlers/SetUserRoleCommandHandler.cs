using System.Security.Authentication;
using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Exceptions;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class SetUserRoleCommandHandler : ICommandHandler<SetUserRoleCommand, NothingnessResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<SetUserRoleCommandHandler> logger;
	
	public SetUserRoleCommandHandler(IUserProxy userProxy, ILogger<SetUserRoleCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(SetUserRoleCommand command)
	{
		var user = userProxy.GetUserById(command.UserId);
		if (user == null)
		{
			logger.LogError($"Unable to find any record for user Id {command.UserId} to set to role level code of {command.RoleLevelCode}");
			throw new UserNotFoundException(command.UserId);
		}
		try
		{
			
			logger.LogInformation($"Request to set role for {user.Username} to role level code of {command.RoleLevelCode}");

			if (!userProxy.AssignRoleTo(user.Id, command.RoleLevelCode))
			{
				logger.LogError($"Cannot find a role level code of {command.RoleLevelCode}");
			}
			else
			{
				logger.LogInformation($"Successful role level change for {user.Username} to role level code of {command.RoleLevelCode}");
			}

			return new NothingnessResponse();
		}
		catch (AuthenticationException ae)
		{
			logger.LogError($"Failure to set role level code for user Id {command.UserId}");
			throw;
		}
	}
}