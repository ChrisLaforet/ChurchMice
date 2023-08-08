using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;

namespace ChurchMiceTesting.Mocks;

public class MockMemberImageRepository : MockRepository<MemberImage, int>, IMemberImageRepository
{
}