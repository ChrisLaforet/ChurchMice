using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Types;

namespace ChurchMiceServer.Domains.Proxies;

public interface IMemberProxy
{
    Member? GetMember(MemberId id);
    IList<Member> FindMembersByName(string lastName, string? firstName = null);
    int CreateMember(Member member);
    void UpdateMember(Member member);
    void RemoveMember(Member member);
    IList<Member> GetMembers();
    MemberImage AddMemberImageFor(Member member, UserId uploadUserId, string compressedImage, string fileType);
    void ApproveMemberImage(MemberImageId memberImageId);
    MemberImage? GetMemberImage(MemberImageId memberImageId);
    List<MemberImage> GetMemberImagesFor(MemberId memberId);
    void RemoveMemberImage(MemberImageId memberImageId);
    void RemoveMemberImagesFor(Member member);
    
    // TODO: Add memberEditor and memberImage lookups and creation here
    int ConnectEditorToMember(User editor, Member member);
    void RemoveEditorFromMember(MemberEditor memberEditor);
    void RemoveEditorFromMember(Member member);
    IList<MemberEditor> GetEditorsForMember(Member member);
    IList<Member> GetMembersForEditor(User user);
}