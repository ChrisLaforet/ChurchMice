using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Security.JWT;

namespace ChurchMiceServer.Domains.Proxies;

public interface IUserProxy
{
	User? GetUserById(string id);
	User? GetUserByGuid(Guid guid);
	User? GetUserByUsername(string username);
	JsonWebToken AuthenticateUser(string username, string password);

	void SetPasswordFor(string username, string resetKey, string password);
	void LogoutUser(string username);
	
	void ExpireUserTokens();
	void DestroyUserToken(JsonWebToken token);
	bool ValidateUserToken(JsonWebToken token);
}