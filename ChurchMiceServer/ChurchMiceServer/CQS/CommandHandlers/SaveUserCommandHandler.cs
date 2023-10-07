using System.Text;
using ChurchMiceServer.Configuration;
using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Exceptions;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Domains.Proxies;
using ChurchMiceServer.Services;
using ChurchMiceServer.Utility;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class SaveUserCommandHandler : ICommandHandler<SaveUserCommand, NothingnessResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<SaveUserCommandHandler> logger;
	
	public SaveUserCommandHandler(IUserProxy userProxy, 
								  ILogger<SaveUserCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}

	public NothingnessResponse Handle(SaveUserCommand command)
	{
		logger.LogInformation($"Request to update user record for Id of {command.Id}");
		var user = userProxy.GetUserById(command.Id);
		if (user == null)
		{
			logger.LogInformation($"Unable to load a user record for user with Id of {command.Id}");
			return new NothingnessResponse();
		}
		
		ValidateUserData(command);

		userProxy.UpdateUser(command.Id, command.UserName, command.FullName, command.Email);

		return new NothingnessResponse();
	}

	private void ValidateUserData(SaveUserCommand command)
	{
		if (command.UserName.Length < 2)
		{
			logger.LogError($"Attempt to save user with invalid username of {command.UserName}");
			throw new InvalidFieldException("UserName");
		}
		
		if (!EmailAddressValidation.IsValidEmail(command.Email))
		{
			logger.LogError($"Attempt to save user {command.UserName} with invalid Email address of {command.Email}");
			throw new InvalidFieldException("Email");
		}
	}
}