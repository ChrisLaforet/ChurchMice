using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;

namespace ChurchMiceTesting.Mocks;

public class MockMemberRepository : MockRepository<Member, int>, IMemberRepository
{
}