using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Persistence;

namespace ChurchMiceServer.Domains.Repositories;

public class UserRoleRepository : Repository<UserRole, int>, IUserRoleRepository
{
    public UserRoleRepository(ChurchMiceContext context) : base(context) {}
}