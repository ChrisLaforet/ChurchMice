using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Domains.Proxies;
using ChurchMiceServer.Security.Auth;
using ChurchMiceServer.Types;

namespace ChurchMiceServer.CQS.QueryHandlers;

public class EditableMembersQueryHandler : IQueryHandler<EditableMembersQuery, IList<MemberResponse>>
{
    private readonly IUserProxy userProxy;
    private readonly IMemberProxy memberProxy;
    private readonly ILogger<MemberQueryHandler> logger;

    public EditableMembersQueryHandler(IUserProxy userProxy, IMemberProxy memberProxy, ILogger<MemberQueryHandler> logger)
    {
        this.userProxy = userProxy;
        this.memberProxy = memberProxy;
        this.logger = logger;
    }
    
    public IList<MemberResponse> Handle(EditableMembersQuery query)
    {
        var members = GetEditableMembersFor(query);
        return CreateResponseFrom(members);
    }

    private IList<MemberResponse> CreateResponseFrom(IList<Member> members)
    {
        var response = new List<MemberResponse>();
        foreach (var member in members)
        {
            response.Add(new MemberResponse(member));
        }
        return response;
    }
    
    private IList<Member> GetEditableMembersFor(EditableMembersQuery query)
    {
        var editor = userProxy.GetUserByUsername(query.Username);

        var role = userProxy.GetAssignedRoleLevelCodeFor(UserId.From(editor.Id));
        if (role == Role.GetAdministrator().Code)
        {
            return memberProxy.GetMembers();
        }

        return memberProxy.GetMembersForEditor(editor);
    }
}