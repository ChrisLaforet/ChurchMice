using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;

namespace ChurchMiceTesting.Mocks;

public class MockUserRoleRepository : MockRepository<UserRole, int>, IUserRoleRepository
{
} 