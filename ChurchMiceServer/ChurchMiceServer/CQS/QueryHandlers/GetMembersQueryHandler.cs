using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.QueryHandlers;

public class GetMembersQueryHandler : IQueryHandler<GetMembersQuery, List<MemberResponse>>
{
	private readonly IMemberProxy memberProxy;
	private readonly ILogger<GetUsersQueryHandler> logger;
	
	public GetMembersQueryHandler(IMemberProxy memberProxy, ILogger<GetUsersQueryHandler> logger)
	{
		this.memberProxy = memberProxy;
		this.logger = logger;
	}

	public List<MemberResponse> Handle(GetMembersQuery query)
	{
		var response = new List<MemberResponse>();
		foreach (var member in memberProxy.GetMembers())
		{
			response.Add(new MemberResponse(member));
		}
		return response;
	}
}