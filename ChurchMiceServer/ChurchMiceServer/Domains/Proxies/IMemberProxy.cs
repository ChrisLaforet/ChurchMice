using ChurchMiceServer.Domains.Models;

namespace ChurchMiceServer.Domains.Proxies;

public interface IMemberProxy
{
    Member? GetMember(int id);
    IList<Member> FindMembersByName(string lastName, string? firstName = null);
    int CreateMember(Member member);
    void UpdateMember(Member member);
    void RemoveMember(Member member);
    IList<Member> GetMembers();
    MemberImage AddMemberImageFor(Member member, string uploadUserId, string compressedImage, string fileType);
    void ApproveMemberImage(int memberImageId);
    MemberImage? GetMemberImage(int memberImageId);
    List<MemberImage> GetMemberImagesFor(int memberId);
    void RemoveMemberImage(int memberImageId);
    void RemoveMemberImagesFor(Member member);
    
    // TODO: Add memberEditor and memberImage lookups and creation here
    int ConnectEditorToMember(User editor, Member member);
    void RemoveEditorFromMember(MemberEditor memberEditor);
    void RemoveEditorFromMember(Member member);
    IList<MemberEditor> GetEditorsForMember(Member member);
    IList<Member> GetMembersForEditor(User user);
}