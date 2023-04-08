using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ChurchMiceServer.Configuration;

namespace ChurchMiceServer.Security
{
	public class ValidateClientMiddleware
	{
		private const string APIKEY_HEADER_KEY = "apikey";
		
		private readonly RequestDelegate next;
		private readonly string apikey;

		private readonly IRegisterNonApiService registerNonApiService;
		
		public ValidateClientMiddleware(RequestDelegate next, IConfigurationLoader configurationLoader, IRegisterNonApiService registerNonApiService)
		{
			this.next = next;
			apikey = configurationLoader.GetKeyValueFor(Startup.APIKEY_STRING_KEY);
			this.registerNonApiService = registerNonApiService;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (!isRouteNonApiService(context))
			{
				if (!context.Request.Headers.ContainsKey(APIKEY_HEADER_KEY))
				{
					context.Response.StatusCode = 403; //forbidden 
					await context.Response.WriteAsync("Invalid apikey");
					return;
				}

				var apikeyFromHeader = context.Request.Headers[APIKEY_HEADER_KEY];
				if (apikeyFromHeader != apikey)
				{
					context.Response.StatusCode = 401; // Unauthorized
					await context.Response.WriteAsync("Invalid apikey");
					return;
				}
			}

			// Call the next delegate/middleware in the pipeline.
			await this.next(context);
		}



		private bool isRouteNonApiService(HttpContext context)
		{
			return registerNonApiService.containsRoute((context.Request.Path));
		}
	}
	
	public static class ValidateClientMiddlewareExtensions
	{
		public static IApplicationBuilder UseValidateClient(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ValidateClientMiddleware>();
		}
	}
}
