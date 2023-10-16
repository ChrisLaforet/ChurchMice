﻿using ChurchMiceServer.Domains.Models;

namespace ChurchMiceServer.Domains.Proxies;

public interface IMemberProxy
{
    Member? GetMember(int id);
    IList<Member> FindMembersByName(string lastName, string? firstName = null);
    int CreateMember(Member member);
    void UpdateMember(Member member);
    void RemoveMember(Member member);
    IList<Member> GetMembers();

    
    // TODO: Add memberEditor and memberImage lookups and creation here
    int ConnectEditorToMember(User editor, Member member);
    void RemoveEditorFromMember(MemberEditor memberEditor);
    IList<MemberEditor> GetEditorsForMember(Member member);
    IList<Member> GetMembersForEditor(User user);
}