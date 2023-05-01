namespace ChurchMiceServer.CQS.Queries;

public class MemberQuery : IQuery
{
    public string Id { get; private set; }

    public MemberQuery(string id)
    {
        this.Id = id;
    }
}