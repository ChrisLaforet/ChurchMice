namespace ChurchMiceServer.CQS.Commands;

public class SaveUserCommand : ICommand
{
    public string Id { get; }
    public string Email { get; }
    public string UserName { get; }
    public string FullName { get; }

    public SaveUserCommand(string id, string userName, string fullName, string email)
    {
        this.Id = id;
        this.UserName = userName;
        this.FullName = fullName;
        this.Email = email;
    }
}