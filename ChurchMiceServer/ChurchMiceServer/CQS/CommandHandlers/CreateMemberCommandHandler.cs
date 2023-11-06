using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Mappers;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Domains.Proxies;
using ChurchMiceServer.Security.Auth;
using ChurchMiceServer.Types;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class CreateMemberCommandHandler : ICommandHandler<CreateMemberCommand, MemberResponse>
{
    private readonly IUserProxy userProxy;
    private readonly IMemberProxy memberProxy;
    private readonly ILogger<CreateMemberCommandHandler> logger;
    
    public CreateMemberCommandHandler(IUserProxy userProxy, IMemberProxy memberProxy, ILogger<CreateMemberCommandHandler> logger)
    {
        this.userProxy = userProxy;
        this.memberProxy = memberProxy;
        this.logger = logger;
    }
    
    public MemberResponse Handle(CreateMemberCommand command)
    {
        try
        {
            var creator = userProxy.GetUserByUsername(command.CreatorUsername);
            var member = MemberMappers.MapCommandToMember(command); 
            memberProxy.CreateMember(member);
            ConnectCreatorToMember(creator, member);
            logger.LogInformation($"Created member for {member.FirstName} {member.LastName} by {command.CreatorUsername}");
            return new MemberResponse(member);
        }
        catch (Exception ex)
        {
            logger.LogError("Error updating member", ex);
        }

        return null;
    }

    private void ConnectCreatorToMember(User creator, Member member)
    {
        var role = userProxy.GetAssignedRoleLevelCodeFor(UserId.From(creator.Id));
        if (role == Role.GetAdministrator().Code)
        {
            return;
        }
        memberProxy.ConnectEditorToMember(creator, member);
    }
}