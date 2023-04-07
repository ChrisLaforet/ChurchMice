using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Security.JWT;

namespace ChurchMiceServer.Domains.Proxies;

public interface IUserProxy
{
	User? GetUserById(String id);
	User? GetUserByGuid(Guid guid);
	User? GetUserByUsername(String username);
	JsonWebToken AuthenticateUser(string username, string password);
	
	void ExpireUserTokens();
	void DestroyUserToken(JsonWebToken token);
	bool ValidateUserToken(JsonWebToken token);
}