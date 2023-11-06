using ChurchMiceServer.Controllers.Models;
using ChurchMiceServer.Types;

namespace ChurchMiceServer.CQS.Commands;

public class UpdateMemberCommand : ICommand
{
    public MemberId Id { get; set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? Email { get; private set; }
    public string? HomePhone { get; private set; }
    public string? MobilePhone { get; private set; }
    public string? MailingAddress1 { get; private set; }
    public string? MailingAddress2 { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? Zip { get; private set; }
    public string? Birthday { get; private set; }
    public string? Anniversary { get; private set; }
    public string? MemberSince { get; private set; }
    public string? UserId { get; private set; }
    
    public string CreatorUsername { get; private set; }

    public UpdateMemberCommand(MemberRequest request, string creatorUsername)
    {
        if (request.Id == null)
        {
            throw new InvalidDataException("Missing required member Id");
        }
        this.Id = MemberId.From((int)request.Id);
        this.FirstName = request.FirstName;
        this.LastName = request.LastName;
        this.Email = request.Email;
        this.HomePhone = request.HomePhone;
        this.MobilePhone = request.MobilePhone;
        this.MailingAddress1 = request.MailingAddress1;
        this.MailingAddress2 = request.MailingAddress2;
        this.City = request.City;
        this.State = request.State;
        this.Zip = request.Zip;
        this.Birthday = request.Birthday;
        this.Anniversary = request.Anniversary;
        this.MemberSince = request.MemberSince;
        this.UserId = request.UserId;

        this.CreatorUsername = creatorUsername;
    }

    public UpdateMemberCommand(MemberId id)
    {
        Id = id;
    }
}