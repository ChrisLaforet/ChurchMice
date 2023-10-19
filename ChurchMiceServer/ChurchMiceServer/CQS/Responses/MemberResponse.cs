using ChurchMiceServer.Domains.Models;

namespace ChurchMiceServer.CQS.Responses;

public class MemberResponse
{
    public int Id { get; private set; }
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
    public DateTime Created { get; private set; }
    
    public MemberResponse(Member member)
    {
        this.Id = member.Id;
        this.FirstName = member.FirstName;
        this.LastName = member.LastName;
        this.Email = member.Email;
        this.HomePhone = member.HomePhone;
        this.MobilePhone = member.MobilePhone;
        this.MailingAddress1 = member.MailingAddress1;
        this.MailingAddress2 = member.MailingAddress2;
        this.City = member.City;
        this.State = member.State;
        this.Zip = member.Zip;
        this.Birthday = member.Birthday;
        this.Anniversary = member.Anniversary;
        this.MemberSince = member.MemberSince;
        this.UserId = member.UserId;
        this.Created = member.Created;
    }
}