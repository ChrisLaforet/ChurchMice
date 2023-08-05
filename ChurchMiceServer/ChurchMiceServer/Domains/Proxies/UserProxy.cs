using System.Security.Authentication;
using ChurchMiceServer.Security.JWT;
using System.Security.Cryptography;
using System.Text;
using ChurchMiceServer.Configuration;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Security.Auth;
using ChurchMiceServer.Services;
using Microsoft.IdentityModel.Tokens;

namespace ChurchMiceServer.Domains.Proxies;

public class UserProxy : IUserProxy
{
    public readonly TimeSpan TOKEN_LIFETIME_TIMESPAN = new TimeSpan(1, 0, 0);

    private readonly IChurchMiceContext context;
    private readonly PasswordProcessor passwordProcessor;
    private readonly string emailSender;
    private readonly IEmailProxy emailProxy;

    public UserProxy(IChurchMiceContext context, IEmailProxy emailProxy, IConfigurationLoader configurationLoader)
    {
        this.context = context;
        this.emailProxy = emailProxy;
        this.emailSender = configurationLoader.GetKeyValueFor(IEmailSenderService.SMTP_SENDER);
        this.passwordProcessor = new PasswordProcessor(configurationLoader);
    }

    public User? GetUserByGuid(Guid guid) => GetUserById(guid.ToString());

    public User? GetUserById(String id)
    {
        return context.Users.Find(id);
    }

    public User? GetUserByUsername(string username)
    {
        return context.Users.FirstOrDefault(user => user.Username == username);
    }

    public IList<User> GetUsersByEmail(string email)
    {
        return context.Users.Where(user => user.Email.ToLower() == email.ToLower()).ToList();
    }

    public string CreateUser(User user)
    {
        user.Id = Guid.NewGuid().ToString();
        context.Users.Add(user);
        context.SaveChanges();
        return user.Id;
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

    public void ChangePasswordFor(string email)
    {
        var resetKey = GenerateResetKey();

        foreach (var user in GetUsersByEmail(email))
        {
            user.ResetKey = resetKey;
            user.ResetExpirationDatetime = DateTime.Now.AddDays(5);
            context.Users.Update(user);
            
// TODO: change ChurchMice to name of church once configuration is added
// TODO: add url to the password change screen when configuration is added
            var contents = new StringBuilder();
            contents.Append(
                "A password request for ChurchMice has been created.  If you did not request this, you do not have to do anything.  However, if you did, use your login portal for ChurchMice and select Change Password.");
            contents.Append("\r\n\r\nYour login username is: ");
            contents.Append(user.Username);
            contents.Append("\r\n\r\nUse the following for the ResetKey: ");
            contents.Append(resetKey);
            contents.Append("\r\n");
            
            emailProxy.SendMessageTo(email, emailSender, "Password change requested", contents.ToString());
        }            
        context.SaveChanges();
    }

    private char? GetPrintableCharacter(byte generated)
    {
        var ch = (char)(generated & 0x7f);
        if (char.IsLetterOrDigit(ch) || ch == '-' || ch == ';' || ch == ':' || ch == '/' || ch == '+' || ch == '$' || ch == '#' || ch == '!')
        {
            return ch;
        }
        return null;
    }

    private string GenerateResetKey()
    {
        using (var cryptoProvider = new RNGCryptoServiceProvider())
        {
            var nextByte = new byte[1];
            cryptoProvider.GetBytes(nextByte);

            int length = 35 + ((int)nextByte[0] & 0xf);  // 35 to 50 chars
            var key = new StringBuilder();
            int offset = 0;
            while (offset < length)
            {
                cryptoProvider.GetBytes(nextByte);
                var ch = GetPrintableCharacter(nextByte[0]);
                if (ch != null)
                {
                    key.Append(ch);
                    ++offset;
                }
            }

            return key.ToString();
        }
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

        context.SaveChanges();
    }
}
