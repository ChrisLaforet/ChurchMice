using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;

namespace ChurchMiceTesting.Mocks;

public class MockUserRepository : MockRepository<User, string>, IUserRepository
{
}