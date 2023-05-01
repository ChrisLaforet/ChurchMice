using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.QueryHandlers;

public class MemberQueryHandler : IQueryHandler<MemberQuery, MemberResponse?>
{
    private readonly IMemberProxy memberProxy;
    private readonly ILogger<MemberQueryHandler> logger;

    public MemberQueryHandler(IMemberProxy memberProxy, ILogger<MemberQueryHandler> logger)
    {
        this.memberProxy = memberProxy;
        this.logger = logger;
    }
    
    public MemberResponse? Handle(MemberQuery query)
    {
        var member = memberProxy.GetMemberById(query.Id);
        if (member == null)
        {
            return null;
        }

        return new MemberResponse(member);
    }
}