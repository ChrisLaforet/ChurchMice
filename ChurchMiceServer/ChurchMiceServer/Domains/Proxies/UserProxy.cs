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

    private readonly IRepositoryContext context;
    private readonly PasswordProcessor passwordProcessor;
    private readonly string emailSender;
    private readonly IEmailProxy emailProxy;
    private readonly IConfigurationProxy configurationProxy;
    
    public UserProxy(IRepositoryContext context,
                IEmailProxy emailProxy,
                IConfigurationProxy configurationProxy,
                IConfigurationLoader configurationLoader)
    {
        this.context = context;
        this.emailProxy = emailProxy;
        this.configurationProxy = configurationProxy;
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
        return context.Users.Where(user => user.Email != null && user.Email.ToLower().Equals(email.ToLower())).ToList();
    }

    public string CreateUser(User user)
    {
        user.Id = Guid.NewGuid().ToString();
        context.Users.Add(user);
        context.SaveChanges();

        var role = new UserRole();
        role.RoleLevel = Role.NO_ACCESS_LEVEL;
        role.UserId = user.Id;
        context.UserRoles.Add(role);
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

    public void ValidateEmailForUser(string username, string password)
    {
        var passwordHash = passwordProcessor.HashPassword(password);
        var user = GetUserByUsername(username);
        if (user == null || user.PasswordHash.IsNullOrEmpty() || user.PasswordHash != passwordHash)
        {
            throw new AuthenticationException();
        }

        UpdateRoleForUser(user);
    }

    private void UpdateRoleForUser(User user)
    {
        var roles = context.UserRoles.Where(role => role.UserId == user.Id);
        if (!roles.IsNullOrEmpty())
        {
            // we will not permit a high role to be overwritten
            if (roles.FirstOrDefault(role => role.RoleLevel > Role.ATTENDER_LEVEL) != null)
            {
                return;
            }
            
            foreach (var role in roles)
            {
                context.UserRoles.Remove(role);
            }
        }
        
        var newRole = new UserRole();
        newRole.RoleLevel = Role.ATTENDER_LEVEL;
        newRole.UserId = user.Id;
        context.UserRoles.Add(newRole);
        context.SaveChanges();
    }
    
    private string GenerateTokenKey()
    {
        var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateIV();
        aes.GenerateKey();
        return System.Convert.ToBase64String(aes.Key);
    }

    private List<string> GetRolesFor(User user)
    {
        var possibleRoleLevels = context.UserRoles.Where(role => role.UserId == user.Id).Select(role => role.RoleLevel);
        var roleLevels = new HashSet<int>();
        if (!possibleRoleLevels.IsNullOrEmpty())
        {
            roleLevels = possibleRoleLevels.ToHashSet();
        }

        var roles = new List<string>();
        
        if (!roleLevels.Any() || roleLevels.Max() <= Role.GetNoAccess().Level)
        {
            roles.Add(Role.GetNoAccess().Name);
        }
        
        foreach (var role in Roles.GetAllRolesWithinLevel(roleLevels.Max()))
        {
            roles.Add(role.Name);
        }

        return roles;
    }

    private JsonWebToken CreateJWTFor(User user)
    {
        var userTokenId = Guid.NewGuid(); // jwt serial

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
            GetRolesFor(user),
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

    public string HashPassword(string password)
    {
        return passwordProcessor.HashPassword(password);
    }

    public void ChangePasswordFor(string email)
    {
        var resetKey = GenerateResetKey();

        foreach (var user in GetUsersByEmail(email))
        {
            user.ResetKey = resetKey;
            user.ResetExpirationDatetime = DateTime.Now.AddDays(5);
            context.Users.Update(user);

            // TODO: add url to the password change screen when configuration is added
            var contents = new StringBuilder();
            contents.Append("A password request for ChurchMice software");
            if (!string.IsNullOrEmpty(configurationProxy.GetMinistryName()))
            {
                contents.Append($" for {configurationProxy.GetMinistryName()}");
            }
            contents.Append(" has been created.  If you did not request this, you do not have to do anything.\r\nHowever, if you did, use your login portal for ChurchMice and select Change Password.");
            contents.Append("\r\n\r\nYour login username is: ");
            contents.Append(user.Username);
            contents.Append("\r\n\r\nGo to the following link to Change Password: ");
            contents.Append($"{configurationProxy.GetBaseUrl()}/changePassword");
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

    public string[] GetUserRoles(JsonWebToken token)
    {
// TODO: CML - get the user roles        
        return null;
    }

}
