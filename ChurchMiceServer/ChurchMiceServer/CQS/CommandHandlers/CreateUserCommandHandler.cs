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

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, StringResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<CreateUserCommandHandler> logger;
	
	public CreateUserCommandHandler(IUserProxy userProxy, 
									IEmailProxy emailProxy,
									IConfigurationProxy configurationProxy,
									IConfigurationLoader configurationLoader,
									ILogger<CreateUserCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}

	public StringResponse Handle(CreateUserCommand command)
	{
		logger.LogInformation($"Request to create user for {command.UserName}");
		ValidateUserData(command);

		var user = CreateUserFor(command);
		return new StringResponse(user.Id);
	}

	public User CreateUserFor(CreateUserCommand command)
	{
		var user = new User();
		user.Email = command.Email;
		user.Fullname = command.FullName;
		user.Username = command.UserName;
		user.PasswordHash = "";
		user.CreateDate = DateTime.Now;
		userProxy.CreateUser(user);

		return user;
	}

	private void ValidateUserData(CreateUserCommand command)
	{
		if (command.UserName.Length < 2)
		{
			logger.LogError($"Attempt to create user with invalid username of {command.UserName}");
			throw new InvalidFieldException("UserName");
		}
		
		if (!EmailAddressValidation.IsValidEmail(command.Email))
		{
			logger.LogError($"Attempt to create user {command.UserName} with invalid Email address of {command.Email}");
			throw new InvalidFieldException("Email");
		}
	}
}