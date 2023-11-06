using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Persistence;
using ChurchMiceServer.Types;
using Microsoft.IdentityModel.Tokens;

namespace ChurchMiceServer.Domains.Proxies;

public class MemberProxy : IMemberProxy
{
    private readonly IRepositoryContext context;

    public MemberProxy(IRepositoryContext context)
    {
        this.context = context;
    }
    
    public Member? GetMember(MemberId id)
    {
        return context.Members.FirstOrDefault(member => member.Id == id.Id);
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
        RemoveEditorFromMember(member);
        RemoveMemberImagesFor(member);
        context.Members.Remove(member);
        context.SaveChanges();
    }

    public IList<Member> GetMembers()
    {
        return context.Members.ToList();
    }

    public MemberImage AddMemberImageFor(Member member, UserId uploadUserId, string compressedImage, string fileType)
    {
        var memberImage = new MemberImage();
        memberImage.MemberId = member.Id;
        memberImage.Image = compressedImage;
        memberImage.UploadDate = DateTime.Now;
        memberImage.UploadUserId = uploadUserId;
        memberImage.ImageType = fileType;
        context.MemberImages.Add(memberImage);
        context.SaveChanges();

        return memberImage;
    }

    public void ApproveMemberImage(MemberImageId memberImageId)
    {
        var memberImage = context.MemberImages.SingleOrDefault(record => record.Id == memberImageId.Id);
        if (memberImage != null)
        {
            memberImage.ApproveDate = DateTime.Now;
            context.SaveChanges();
        }
    }

    public MemberImage? GetMemberImage(MemberImageId memberImageId)
    {
        return context.MemberImages.SingleOrDefault(record => record.Id == memberImageId.Id);
    }

    public List<MemberImage> GetMemberImagesFor(MemberId memberId)
    {
        return context.MemberImages.Where(record => record.MemberId == memberId.Id).ToList();
    }

    public void RemoveMemberImage(MemberImageId memberImageId)
    {
        var memberImage = context.MemberImages.SingleOrDefault(record => record.Id == memberImageId.Id);
        if (memberImage != null)
        {
            context.MemberImages.Remove(memberImage);
            context.SaveChanges();
        }
    }

    public void RemoveMemberImagesFor(Member member)
    {
        foreach (var memberImage in context.MemberImages.Where(record => record.MemberId == member.Id))
        {
            context.MemberImages.Remove(memberImage);
        }
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

    public void RemoveEditorFromMember(Member member)
    {
        foreach (var memberEditor in context.MemberEditors.Where(record => record.MemberId == member.Id))
        {
            context.MemberEditors.Remove(memberEditor);
        }

        context.SaveChanges();
    }

    public IList<MemberEditor> GetEditorsForMember(Member member)
    {
        return context.MemberEditors.Where(me => me.MemberId == member.Id).ToList();
    }

    public IList<Member> GetMembersForEditor(User user)
    {
        var matches = context.MemberEditors.Where(me => me.EditorId == user.Id).ToList();
        var list = new List<Member>();
        foreach (var match in matches)
        {
            list.Add(GetMember(MemberId.From(match.MemberId)));
        }
        return list;
    }
}