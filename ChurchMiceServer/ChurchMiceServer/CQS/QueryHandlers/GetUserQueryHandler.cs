using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.QueryHandlers;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserResponse?>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<GetUserQueryHandler> logger;
	
	public GetUserQueryHandler(IUserProxy userProxy, ILogger<GetUserQueryHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}

	public UserResponse? Handle(GetUserQuery query)
	{
		var user = userProxy.GetUserById(query.UserId);
		if (user == null)
		{
			logger.LogInformation($"Unable to find a user with Id of {query.UserId}");
			return null;
		}

		return new UserResponse(user, userProxy.GetAssignedRoleLevelCodeFor(user.Id));
	}
}