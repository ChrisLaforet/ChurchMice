using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.Domains.Models;

namespace ChurchMiceServer.CQS.Mappers;

public class MemberMappers
{
    
    public static Member MapCommandToMember(CreateMemberCommand command)
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
        member.UserId = command.UserId;
        member.Created = DateTime.Now;
        return member;
    }
    
    public static Member MapCommandToMember(UpdateMemberCommand command)
    {
        var member = new Member();
        member.Id = command.Id;
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