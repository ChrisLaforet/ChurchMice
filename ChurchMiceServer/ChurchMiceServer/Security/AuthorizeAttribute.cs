using System.Security.Authentication;
using System.Security.Principal;
using ChurchMiceServer.Domains.Proxies;
using ChurchMiceServer.Security.JWT;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace ChurchMiceServer.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
	public const string AUTHORIZATION_KEY = "authorization";
	public const string BEARER_HEADER = "Bearer ";

	public void OnAuthorization(AuthorizationFilterContext context)
	{
		// Checks the bearer token
		// TODO: CML- finish authorization support??
// 		var user = context.HttpContext.Items["User"];
// 		if (user == null)
// 		{
// 			throw new AuthenticationException("Not authenticated");
// 			// not logged in
// //				context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
// 		}

		var bearer = context.HttpContext.Request.Headers[AUTHORIZATION_KEY];
		if (bearer.IsNullOrEmpty())
		{
			throw new AuthenticationException("No authorization token provided");
		}

		var jwtContent = bearer[0];
		if (jwtContent.StartsWith(BEARER_HEADER))
		{
			jwtContent = jwtContent.Substring(BEARER_HEADER.Length);
		}

		var jwt = JsonWebToken.From(jwtContent);
		var userProxy = context.HttpContext.RequestServices.GetService<IUserProxy>();
		if (userProxy == null || !userProxy.ValidateUserToken(jwt))
		{
			throw new AuthenticationException("Invalid bearer token");
		}

		var identity = new GenericIdentity(jwt.User); 
// TODO: get the roles from jwt when they are created
		var principal = new GenericPrincipal(identity, null);
		Thread.CurrentPrincipal = principal;
		context.HttpContext.User = principal;
		//HttpContext.Current.User = principal;
	}
}
