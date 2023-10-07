namespace ChurchMiceServer.CQS.Commands;

public class SaveUserCommand : ICommand
{
    public string Id { get; }
    public string Email { get; }
    public string FullName { get; }

    public SaveUserCommand(string id, string fullName, string email)
    {
        this.Id = id;
        this.FullName = fullName;
        this.Email = email;
    }
}