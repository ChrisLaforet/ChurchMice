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

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, NothingnessResponse>
{
	private readonly IUserProxy userProxy;
	private readonly IEmailProxy emailProxy;
	private readonly IConfigurationProxy configurationProxy;
	private readonly string emailSender;
	private readonly ILogger<CreateUserCommandHandler> logger;
	
	public CreateUserCommandHandler(IUserProxy userProxy, 
									IEmailProxy emailProxy,
									IConfigurationProxy configurationProxy,
									IConfigurationLoader configurationLoader,
									ILogger<CreateUserCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.emailProxy = emailProxy;
		this.configurationProxy = configurationProxy;
		this.emailSender = configurationLoader.GetKeyValueFor(IEmailSenderService.SMTP_SENDER);
		this.logger = logger;
	}

	public NothingnessResponse Handle(CreateUserCommand command)
	{
		logger.LogInformation($"Request to create login for {command.UserName}");
		ValidateUserData(command);

		var user = CreateUserFor(command);
		RequestEmailValidationFrom(user);
		return new NothingnessResponse();
	}

	public User CreateUserFor(CreateUserCommand command)
	{
		var user = new User();
		//user.Id = Guid.NewGuid().ToString();
		user.Email = command.Email;
		user.Fullname = command.FullName;
		user.Username = command.UserName;
		user.PasswordHash = userProxy.HashPassword(command.Password);
		user.CreateDate = DateTime.Now;
		userProxy.CreateUser(user);

		return user;
	}

	private void RequestEmailValidationFrom(User user) 
	{
		var contents = new StringBuilder();
		contents.Append("A new account has been requested in ChurchMice software");
		if (!string.IsNullOrEmpty(configurationProxy.GetMinistryName()))
		{
			contents.Append($" for {configurationProxy.GetMinistryName()}");
		}
		contents.Append(" and has been created.  If you did not request this, you do not have to do anything.\r\nHowever, if you did, please validate your Email address with us before being granted access.");
		contents.Append("\r\n\r\nYour login username is: ");
		contents.Append(user.Username);
		contents.Append("\r\n\r\nClick the following link to validate your Email address: ");
		contents.Append($"{configurationProxy.GetBaseUrl()}/validateUserEmail");
		contents.Append("\r\n");
		
		emailProxy.SendMessageTo(user.Email, emailSender, "Please confirm your Email", contents.ToString());
	}

	private void ValidateUserData(CreateUserCommand command)
	{
		if (command.UserName.Length < 2)
		{
			logger.LogError($"Attempt to create user with invalid username of {command.UserName}");
			throw new InvalidFieldException("UserName");
		}

		if (command.Password.Length < 8)
		{
			logger.LogError($"Attempt to create user {command.UserName} with password that does not meet criteria");
			throw new InvalidFieldException("Password");
		}
		
		if (!EmailAddressValidation.IsValidEmail(command.Email))
		{
			logger.LogError($"Attempt to create user {command.UserName} with invalid Email address of {command.Email}");
			throw new InvalidFieldException("Email");
		}
	}
}