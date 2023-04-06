using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChurchMiceServer.Security
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class AuthorizeAttribute : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			// TODO: finish authorization support??
			// var user = (User)context.HttpContext.Items["User"];
			// if (user == null)
			// {
			// 	// not logged in
			// 	context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
			// }
		}
	}
}