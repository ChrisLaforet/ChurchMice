using System.Security.Authentication;
using ChurchMiceServer.Security.JWT;
using System.Security.Cryptography;
using ChurchMiceServer.Configuration;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Security.Auth;
using Microsoft.IdentityModel.Tokens;

namespace ChurchMiceServer.Domains.Proxies;

public class UserProxy : IUserProxy
{
    public readonly TimeSpan TOKEN_LIFETIME_TIMESPAN = new TimeSpan(1, 0, 0);

    private readonly ChurchMiceContext context;
    private readonly PasswordProcessor passwordProcessor;

    public UserProxy(ChurchMiceContext context, IConfigurationLoader configurationLoader)
    {
        this.context = context;
        this.passwordProcessor = new PasswordProcessor(configurationLoader);
    }

    public User? GetUserByGuid(Guid guid) => GetUserById(guid.ToString());

    public User? GetUserById(String id)
    {
        return context.Users.Find(id);
    }

    public User? GetUserByUsername(string username)
    {
        return context.Users.Where(user => user.Username == username).FirstOrDefault();
    }


    public JsonWebToken AuthenticateUser(string username, string password)
    {
        var passwordHash = passwordProcessor.HashPassword(password);
        var user = GetUserByUsername(username);
        if (user == null || user.PasswordHash.IsNullOrEmpty() || user.PasswordHash != passwordHash)
        {
            throw new AuthenticationException();
        }
        
        return CreateJWTFor(user);
    }

    private string GenerateTokenKey()
    {
        var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateIV();
        aes.GenerateKey();
        return System.Convert.ToBase64String(aes.Key);
    }

    private JsonWebToken CreateJWTFor(User user)
    {
        var userTokenId = Guid.NewGuid(); // jwt serial
        var roles = new List<string>();
// TODO: CML - get role level and assign        
roles.Add("READ");
roles.Add("WRITE");

        var userToken = new UserToken();
        userToken.Id = userTokenId.ToString();
        userToken.UserId = user.Id;
        userToken.TokenKey = GenerateTokenKey();
        userToken.Created = DateTime.Now;
        userToken.Expired = userToken.Created.Add(TOKEN_LIFETIME_TIMESPAN);
        context.UserTokens.Add(userToken);
        context.SaveChanges();

        return JsonWebToken.New(userToken.TokenKey, userTokenId, 
            user.Username,
            user.Id,
            roles, 
            userToken.Expired);
    }

    public void ExpireUserTokens()
    {
        foreach (var userToken in context.UserTokens.Where(token => token.Expired < DateTime.Now))
        {
            context.Remove(userToken);
        }

        context.SaveChanges();
    }

    public void DestroyUserToken(JsonWebToken token)
    {
        var userToken = context.UserTokens.Find(token.Serial);
        if (userToken != null)
        {
            try
            {
                token.AssertTokenIsValid(userToken.TokenKey);
                context.Remove(userToken);
                context.SaveChanges();
            }
            catch (Exception)
            {
                // do nothing since token is not valid
            }
        }
    }

    public bool ValidateUserToken(JsonWebToken token)
    {
        var userToken = context.UserTokens.Find(token.Serial);
        if (userToken != null)
        {
            try
            {
                token.AssertTokenIsValid(userToken.TokenKey);
                token.AssertTokenIsExpired();
                return true;
            }
            catch (Exception)
            {
                // do nothing but pass through to false
            }
        }

        return false;
    }

    public void SetPasswordFor(string username, string resetKey, string password)
    {
        var user = GetUserByUsername(username);
        if (user == null || user.ResetKey.IsNullOrEmpty())
        {
            throw new AuthenticationException();
        }
        
        if (user.ResetKey != resetKey || user.ResetExpirationDatetime == null || user.ResetExpirationDatetime <= DateTime.Now)
        {
            throw new AuthenticationException();
        }
        
        var passwordHash = passwordProcessor.HashPassword(password);

        // save password
        user.PasswordHash = passwordHash;
        user.ResetKey = null;
        user.ResetExpirationDatetime = null;
        context.Users.Update(user);
        
        // force user to have to relogin
        LogoutUser(username);

        context.SaveChanges();
    }

    public void LogoutUser(string username)
    {
        var user = GetUserByUsername(username);
        if (user != null)
        {
            foreach (var userToken in context.UserTokens.Where(token => token.UserId == user.Id))
            {
                context.UserTokens.Remove(userToken);
            }
        }
    }
}
