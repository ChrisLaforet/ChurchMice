namespace ChurchMiceServer.Controllers.Models;

public class JwtResponse
{
    public string Token { get; private set; }
		
    public string Username { get; private set; }
    
    public string Fullname { get; private set; }
		
    public string Email { get; private set; }

    public JwtResponse(string token, string username, string fullname, string email)
    {
        this.Token = token;
        this.Username = username;
        this.Fullname = fullname;
        this.Email = email;
    }
}