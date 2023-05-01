using ChurchMiceServer.Domains.Models;

namespace ChurchMiceServer.Domains.Proxies;

public interface IMemberProxy
{
    Member? GetMemberById(string id);
    IList<Member> FindMembersByName(string lastName, string? firstName = null);
    int CreateMember(Member member);
    void UpdateMember(Member member);
    void RemoveMember(Member member);
}