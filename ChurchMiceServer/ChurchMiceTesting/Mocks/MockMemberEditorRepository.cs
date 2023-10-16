using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;

namespace ChurchMiceTesting.Mocks;

public class MockMemberEditorRepository : MockRepository<MemberEditor, int>, IMemberEditorRepository
{
}