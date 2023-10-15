using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Persistence;
using Microsoft.IdentityModel.Tokens;

namespace ChurchMiceServer.Domains.Proxies;

public class MemberProxy : IMemberProxy
{
    private readonly IRepositoryContext context;

    public MemberProxy(IRepositoryContext context)
    {
        this.context = context;
    }
    
    public Member? GetMember(int id)
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

    public int ConnectEditorToMember(User editor, Member member)
    {
        var record = context.MemberEditors.FirstOrDefault(me => me.MemberId == member.Id && me.EditorId == editor.Id);
        if (record != null)
        {
            return record.Id;
        }

        record = new MemberEditor() { MemberId = member.Id, EditorId = editor.Id };
        context.MemberEditors.Add(record);
        context.SaveChanges();
        return record.Id;
    }

    public void RemoveEditorFromMember(MemberEditor memberEditor)
    {
        context.MemberEditors.Remove(memberEditor);
        context.SaveChanges();
    }

    public IList<MemberEditor> GetEditorsForMember(Member member)
    {
        return context.MemberEditors.Where(me => me.MemberId == member.Id).ToList();
    }

    public IList<MemberEditor> GetMembersForEditor(User user)
    {
        return context.MemberEditors.Where(me => me.EditorId == user.Id).ToList();
    }
}