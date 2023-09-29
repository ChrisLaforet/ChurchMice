using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.QueryHandlers;

public class CheckExistingNameQueryHandler : IQueryHandler<CheckExistingNameQuery, BooleanResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<CheckExistingNameQueryHandler> logger;
	
	public CheckExistingNameQueryHandler(IUserProxy userProxy, ILogger<CheckExistingNameQueryHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public BooleanResponse Handle(CheckExistingNameQuery query)
	{
		logger.Log(LogLevel.Information, "Request to check " + query.CheckField + " for " + query.CheckValue);
		try
		{
			if (CheckForMatchingUser(query))
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

	private bool CheckForMatchingUser(CheckExistingNameQuery query)
	{
		if (query.CheckField.ToUpper().Equals(CheckExistingNameQuery.CHECK_USERNAME))
		{
			return userProxy.GetUserByUsername(query.CheckValue) != null;
		}
		if (query.CheckField.ToUpper().Equals(CheckExistingNameQuery.CHECK_EMAIL))
		{
			return userProxy.GetUsersByEmail(query.CheckValue).Count > 0;
		}

		return false;
	}
}