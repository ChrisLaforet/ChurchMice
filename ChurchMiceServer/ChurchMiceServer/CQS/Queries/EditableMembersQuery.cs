namespace ChurchMiceServer.CQS.Queries;

public class EditableMembersQuery : IQuery
{
    public string Username { get; private set; }

    public EditableMembersQuery(string Username)
    {
        this.Username = Username;
    }
}