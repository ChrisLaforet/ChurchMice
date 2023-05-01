using ChurchMiceServer.Domains.Models;
using Microsoft.IdentityModel.Tokens;

namespace ChurchMiceServer.Domains.Proxies;

public class MemberProxy : IMemberProxy
{
    private readonly ChurchMiceContext context;

    public MemberProxy(ChurchMiceContext context)
    {
        this.context = context;
    }
    
    public Member? GetMemberById(int id)
    {
        return context.Members.FirstOrDefault(member => member.Id.Equals(id));
    }

    public IList<Member> FindMembersByName(string lastName, string? firstName = null)
    {
        if (firstName.IsNullOrEmpty())
        {
            return context.Members.Where(member => member.LastName.ToLower() == lastName.ToLower()).ToList();
        }
        return context.Members
            .Where(member => member.LastName.ToLower() == lastName.ToLower() && 
                             member.FirstName.ToLower() == firstName.ToLower())
            .ToList();
    }

    public int CreateMember(Member member)
    {
        context.Members.Add(member);
        context.SaveChanges();
        return member.Id;
    }

    public void UpdateMember(Member member)
    {
        context.Members.Update(member);
        context.SaveChanges();
    }

    public void RemoveMember(Member member)
    {
        context.Members.Remove(member);
        context.SaveChanges();
    }
}