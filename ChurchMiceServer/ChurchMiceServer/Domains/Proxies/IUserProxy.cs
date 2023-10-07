﻿using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Security.JWT;

namespace ChurchMiceServer.Domains.Proxies;

public interface IUserProxy
{
	User? GetUserById(string id);
	User? GetUserByGuid(Guid guid);
	User? GetUserByUsername(string username);
	IList<User> GetUsersByEmail(string email);
	IList<User> GetUsers();
	JsonWebToken AuthenticateUser(string username, string password);
	string CreateUser(User user);
	void ValidateEmailForUser(string username, string password);
	void UpdateUser(string userId, string userName, string fullName, string email);

	void SetPasswordFor(string username, string resetKey, string password);
	string HashPassword(string password);
	void ChangePasswordFor(string username);
	void LogoutUser(string username);
	
	void ExpireUserTokens();
	void DestroyUserToken(JsonWebToken token);
	bool ValidateUserToken(JsonWebToken token);
	string[] GetUserRoles(JsonWebToken token);
	string GetAssignedRoleLevelCodeFor(string userId);
	bool AssignRoleTo(string userId, string roleLevelCode);
}