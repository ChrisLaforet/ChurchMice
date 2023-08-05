using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Persistence;

namespace ChurchMiceServer.Domains.Repositories;

public class UserRoleRepository : Repository<UserRole, Int32>, IUserRoleRepository
{
    public UserRoleRepository(ChurchMiceContext context) : base(context) {}
}