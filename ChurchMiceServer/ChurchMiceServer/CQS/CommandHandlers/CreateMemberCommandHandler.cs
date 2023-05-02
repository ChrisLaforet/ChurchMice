using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Models;
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
            var member = CreateMember(command); 
            memberProxy.CreateMember(member);
            logger.Log(LogLevel.Information, string.Format("Created member for {0} {1} for {2}", member.FirstName, member.LastName, command.CreatorUsername));
            return new MemberResponse(member);
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, "Error creating member", ex);
        }

        return null;
    }

    private static Member CreateMember(CreateMemberCommand command)
    {
        var member = new Member();
        member.FirstName = command.FirstName;
        member.LastName = command.LastName;
        member.Email = command.Email;
        member.HomePhone = command.HomePhone;
        member.MobilePhone = command.MobilePhone;
        member.MailingAddress1 = command.MailingAddress1;
        member.MailingAddress2 = command.MailingAddress2;
        member.City = command.City;
        member.State = command.State;
        member.Zip = command.Zip;
        member.Anniversary = command.Anniversary;
        member.Birthday = command.Birthday;
        //member.UserId
        member.Created = DateTime.Now;
        return member;
    }
}