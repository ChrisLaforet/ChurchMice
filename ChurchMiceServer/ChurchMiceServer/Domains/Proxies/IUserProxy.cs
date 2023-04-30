using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Security.JWT;

namespace ChurchMiceServer.Domains.Proxies;

public interface IUserProxy
{
	User? GetUserById(string id);
	User? GetUserByGuid(Guid guid);
	User? GetUserByUsername(string username);
	IList<User> GetUsersByEmail(string email);
	JsonWebToken AuthenticateUser(string username, string password);

	void SetPasswordFor(string username, string resetKey, string password);
	void ChangePasswordFor(string email);
	void LogoutUser(string username);
	
	void ExpireUserTokens();
	void DestroyUserToken(JsonWebToken token);
	bool ValidateUserToken(JsonWebToken token);
}