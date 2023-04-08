using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.Services;

using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;


public class UserAuthenticationService : IAuthenticationService
{
    private IUserProxy userProxy;

    public UserAuthenticationService(IUserProxy userProxy)
    {
        this.userProxy = userProxy;
    }

    public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
    {
        throw new NotImplementedException();
    }

    public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        return Task.CompletedTask;
    }

    public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }

    public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }

    public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }
}
