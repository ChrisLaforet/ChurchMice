using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Persistence;

namespace ChurchMiceServer.Domains.Repositories;

public class UserRepository : Repository<User, string>, IUserRepository
{
    public UserRepository(ChurchMiceContext context) : base(context) {}
}