using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Persistence;

namespace ChurchMiceServer.Domains.Repositories;

public class UserTokenRepository : Repository<UserToken, string>, IUserTokenRepository
{
    public UserTokenRepository(ChurchMiceContext context) : base(context) {}
}