﻿using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Mappers;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class CreateMemberCommandHandler : ICommandHandler<CreateMemberCommand, MemberResponse>
{
    private readonly IMemberProxy memberProxy;
    private readonly ILogger<CreateMemberCommandHandler> logger;
    
    public CreateMemberCommandHandler(IMemberProxy memberProxy, ILogger<CreateMemberCommandHandler> logger)
    {
        this.memberProxy = memberProxy;
        this.logger = logger;
    }
    
    public MemberResponse Handle(CreateMemberCommand command)
    {
        try
        {
            var member = MemberMappers.MapCommandToMember(command); 
            memberProxy.CreateMember(member);
            logger.Log(LogLevel.Information, string.Format("Created member for {0} {1} by {2}", member.FirstName, member.LastName, command.CreatorUsername));
            return new MemberResponse(member);
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, "Error updating member", ex);
        }

        return null;
    }
}