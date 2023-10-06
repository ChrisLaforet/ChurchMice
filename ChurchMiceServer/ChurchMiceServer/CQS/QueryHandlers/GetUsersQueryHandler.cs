using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.QueryHandlers;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, UsersResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<GetUsersQueryHandler> logger;
	
	public GetUsersQueryHandler(IUserProxy userProxy, ILogger<GetUsersQueryHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}

	public UsersResponse Handle(GetUsersQuery query)
	{
		var users = new List<UserResponse>();
		foreach (var user in userProxy.GetUsers())
		{
			users.Add(new UserResponse(user, userProxy.GetAssignedRoleLevelCodeFor(user.Id)));
		}
		return new UsersResponse(users);
	}
}