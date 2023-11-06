using ChurchMiceServer.Types;

namespace ChurchMiceServer.CQS.Commands;

public class SaveUserCommand : ICommand
{
    public UserId Id { get; }
    public string Email { get; }
    public string FullName { get; }

    public SaveUserCommand(UserId id, string fullName, string email)
    {
        this.Id = id;
        this.FullName = fullName;
        this.Email = email;
    }
}