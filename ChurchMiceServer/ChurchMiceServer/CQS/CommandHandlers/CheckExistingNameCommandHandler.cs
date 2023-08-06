using System.Security.Authentication;
using ChurchMiceServer.Controllers.Models;
using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class CheckExistingNameCommandHandler : ICommandHandler<CheckExistingNameCommand, BooleanResponse>
{
	private const string CHECK_USERNAME = "USERNAME";
	private const string CHECK_EMAIL = "EMAIL";
	
	private readonly IUserProxy userProxy;
	private readonly ILogger<CheckExistingNameCommandHandler> logger;
	
	public CheckExistingNameCommandHandler(IUserProxy userProxy, ILogger<CheckExistingNameCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public BooleanResponse Handle(CheckExistingNameCommand command)
	{
		logger.Log(LogLevel.Information, "Request to check " + command.CheckField + " for " + command.CheckValue);
		try
		{
			if (CheckForMatchingUser(command))
			{
				return new BooleanResponse(false);
			}
		}
		catch (Exception ex)
		{
			logger.Log(LogLevel.Error, "Caught an exception during request to change password: " + ex);
		}
		return new BooleanResponse(true);
	}

	private bool CheckForMatchingUser(CheckExistingNameCommand command)
	{
		if (command.CheckField.ToUpper().Equals(CHECK_USERNAME))
		{
			return userProxy.GetUserByUsername(command.CheckValue) != null;
		}
		if (command.CheckField.ToUpper().Equals(CHECK_EMAIL))
		{
			return userProxy.GetUsersByEmail(command.CheckValue).Count > 0;
		}

		return false;
	}
}