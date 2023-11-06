using ChurchMiceServer.CQS.Exceptions;
using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Domains.Proxies;
using ChurchMiceServer.Security.Auth;
using ChurchMiceServer.Types;

namespace ChurchMiceServer.CQS.QueryHandlers;

public class GetMemberImagesQueryHandler : IQueryHandler<GetMemberImagesQuery, MemberImagesResponse>
{
    private readonly IUserProxy userProxy;
    private readonly IMemberProxy memberProxy;
    private readonly ILogger<GetMemberImagesQueryHandler> logger;
    
    public GetMemberImagesQueryHandler(IUserProxy userProxy, IMemberProxy memberProxy, ILogger<GetMemberImagesQueryHandler> logger)
    {
        this.userProxy = userProxy;
        this.memberProxy = memberProxy;
        this.logger = logger;
    }
    
    public MemberImagesResponse Handle(GetMemberImagesQuery query)
    {
        var response = new MemberImagesResponse();
        var member = memberProxy.GetMember(query.MemberId);
        if (!CheckIfUserIsPermittedToGetImages(query.UserId, member))
        {
            return response;
        }

        foreach (var image in memberProxy.GetMemberImagesFor(query.MemberId))
        {
            response.Add(image);
        }

        return response;
    }
    
    private bool CheckIfUserIsPermittedToGetImages(string userId, Member member)
    {
        // is the user (regardless of permission) the member?
        if (member.UserId != null && member.UserId == userId)
        {
            return true;
        }

        return userProxy.GetAssignedRoleLevelCodeFor(UserId.From(userId)) != Role.GetNoAccess().Code;
    }
}