using ChurchMiceServer.Domains.Models;

namespace ChurchMiceServer.Domains.Interfaces;

public interface IUserTokenRepository : IRepository<UserToken, string>
{
    
}