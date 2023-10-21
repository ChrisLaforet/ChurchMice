using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class DeleteMemberCommandHandler : ICommandHandler<DeleteMemberCommand, NothingnessResponse>
{
    private readonly IUserProxy userProxy;
    private readonly IMemberProxy memberProxy;
    private readonly ILogger<CreateMemberCommandHandler> logger;
    
    public DeleteMemberCommandHandler(IUserProxy userProxy, IMemberProxy memberProxy, ILogger<CreateMemberCommandHandler> logger)
    {
        this.userProxy = userProxy;
        this.memberProxy = memberProxy;
        this.logger = logger;
    }
    
    public NothingnessResponse Handle(DeleteMemberCommand command)
    {
        try
        {
            var member = memberProxy.GetMember(command.MemberId);
            memberProxy.RemoveMember(member);
            logger.LogInformation($"Deleted member record for {member.FirstName} {member.LastName} by {command.CreatorUsername}");
            return new NothingnessResponse();
        }
        catch (Exception ex)
        {
            logger.LogError($"Caught exception deleting member {command.MemberId}: {ex}");
            throw;
        }
    }
}