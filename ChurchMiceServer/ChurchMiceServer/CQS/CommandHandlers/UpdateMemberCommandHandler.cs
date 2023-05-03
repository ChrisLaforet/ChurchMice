using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Mappers;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class UpdateMemberCommandHandler : ICommandHandler<UpdateMemberCommand, MemberResponse>
{
    private readonly IMemberProxy memberProxy;
    private readonly ILogger<UpdateMemberCommandHandler> logger;
    
    public UpdateMemberCommandHandler(IMemberProxy memberProxy, ILogger<UpdateMemberCommandHandler> logger)
    {
        this.memberProxy = memberProxy;
        this.logger = logger;
    }
    
    public MemberResponse Handle(UpdateMemberCommand command)
    {
        try
        {
            var member = MemberMappers.MapCommandToMember(command); 
            memberProxy.CreateMember(member);
            logger.Log(LogLevel.Information, string.Format("Updated member for {0} {1} by {2}", member.FirstName, member.LastName, command.CreatorUsername));
            return new MemberResponse(member);
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, "Error creating member", ex);
        }

        return null;
    }
}