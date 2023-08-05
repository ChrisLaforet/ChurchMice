using ChurchMiceServer.Domains.Models;

namespace ChurchMiceServer.Domains.Interfaces;

public interface IUserRepository : IRepository<User, string>
{
    
}