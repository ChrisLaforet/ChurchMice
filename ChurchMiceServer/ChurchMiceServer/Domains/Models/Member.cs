namespace ChurchMiceServer.Domains.Models;

public class Member
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string HomePhone { get; set; }
    public string MobilePhone { get; set; }
    public string MailingAddress1 { get; set; }
    public string MailingAddress2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Birthday { get; set; }
    public string Anniversary { get; set; }
    public string MemberSince { get; set; }
    public string Created { get; set; }
    public string UserId { get; set; }
}