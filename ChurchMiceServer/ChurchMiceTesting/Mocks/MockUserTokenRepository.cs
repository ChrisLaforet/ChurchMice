using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;

namespace ChurchMiceTesting.Mocks;

public class MockUserTokenRepository : MockRepository<UserToken, string>, IUserTokenRepository
{
}  
