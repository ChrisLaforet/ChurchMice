using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Types;

namespace ChurchMiceServer.CQS.Mappers;

public class MemberMappers
{
    
    public static Member MapCommandToMember(CreateMemberCommand command)
    {
        var member = new Member();
        member.FirstName = command.FirstName;
        member.LastName = command.LastName;
        member.Email = command.Email;

        member.HomePhone = ValidateAndCompressPhone(command.HomePhone);
        member.MobilePhone = ValidateAndCompressPhone(command.MobilePhone);
        member.MailingAddress1 = command.MailingAddress1;
        member.MailingAddress2 = command.MailingAddress2;
        member.City = command.City;
        member.State = command.State;
        member.Zip = command.Zip;
        member.Anniversary = command.Anniversary;
        member.Birthday = command.Birthday;
        member.UserId = command.UserId;
        member.MemberSince = command.MemberSince;
        member.Created = DateTime.Now;
        return member;
    }

    private static string? ValidateAndCompressPhone(string? value)
    {
        try
        {
            var phone = new PhoneNumber(value);
            return phone.GetCompressed();
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    public static Member MapCommandToMember(UpdateMemberCommand command)
    {
        var member = new Member();
        member.Id = command.Id;
        member.FirstName = command.FirstName;
        member.LastName = command.LastName;
        member.Email = command.Email;
        member.HomePhone = ValidateAndCompressPhone(command.HomePhone);
        member.MobilePhone = ValidateAndCompressPhone(command.MobilePhone);
        member.MailingAddress1 = command.MailingAddress1;
        member.MailingAddress2 = command.MailingAddress2;
        member.City = command.City;
        member.State = command.State;
        member.Zip = command.Zip;
        member.Anniversary = command.Anniversary;
        member.Birthday = command.Birthday;
        member.MemberSince = command.MemberSince;
        member.UserId = command.UserId;
        member.Created = DateTime.Now;
        return member;
    }
}