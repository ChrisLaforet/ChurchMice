using ChurchMiceServer.Security.JWT;

namespace ChurchMiceServer.CQS.Responses;

public class LoginResponse
{
	public JsonWebToken Token
	{
		get;
	}

	public LoginResponse(JsonWebToken token) => this.Token = token;
}