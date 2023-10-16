using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Exceptions;
using ChurchMiceServer.CQS.Mappers;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Domains.Proxies;
using ChurchMiceServer.Security.Auth;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class UpdateMemberCommandHandler : ICommandHandler<UpdateMemberCommand, MemberResponse>
{
    private readonly IUserProxy userProxy;
    private readonly IMemberProxy memberProxy;
    private readonly ILogger<UpdateMemberCommandHandler> logger;
    
    public UpdateMemberCommandHandler(IUserProxy userProxy, IMemberProxy memberProxy, ILogger<UpdateMemberCommandHandler> logger)
    {
        this.userProxy = userProxy;
        this.memberProxy = memberProxy;
        this.logger = logger;
    }
    
    public MemberResponse Handle(UpdateMemberCommand command)
    {
        try
        {
            var member = memberProxy.GetMember(command.Id);
            ValidateEditorHasPermission(command, member);
            UpdateMemberValues(member, command);
            logger.LogInformation(string.Format("Updated member for {0} {1} by {2}", member.FirstName, member.LastName, command.CreatorUsername));
            return new MemberResponse(member);
        }
        catch (Exception ex)
        {
            logger.LogError("Error updating member", ex);
        }

        return null;
    }

    private void UpdateMemberValues(Member member, UpdateMemberCommand command)
    {
        var changes = MemberMappers.MapCommandToMember(command);
        MergeMemberChanges(changes, member);
        memberProxy.UpdateMember(member);
    }

    private void MergeMemberChanges(Member changes, Member destination)
    {
        if (!string.IsNullOrEmpty(changes.FirstName))
        {
            destination.FirstName = changes.FirstName;
        }
        if (!string.IsNullOrEmpty(changes.LastName))
        {
            destination.LastName = changes.LastName;
        }
        destination.Email = changes.Email;
        destination.HomePhone = changes.HomePhone;
        destination.MobilePhone = changes.MobilePhone;
        destination.MailingAddress1 = changes.MailingAddress1;
        destination.MailingAddress2 = changes.MailingAddress2;
        destination.City = changes.City;
        destination.State = changes.State;
        destination.Zip = changes.Zip;
        destination.Anniversary = changes.Anniversary;
        destination.Birthday = changes.Birthday;
    }

    private void ValidateEditorHasPermission(UpdateMemberCommand command, Member member)
    {
        var editor = userProxy.GetUserByUsername(command.CreatorUsername);
        var role = userProxy.GetAssignedRoleLevelCodeFor(editor.Id);
        if (role == Role.GetAdministrator().Code)
        {
            return;
        }

        if (memberProxy.GetEditorsForMember(member).FirstOrDefault(me => me.EditorId == editor.Id) != null)
        {
            return;
        }

        throw new UserNotPermittedxception(editor.Id, $"edit member {member.Id}");
    }
}