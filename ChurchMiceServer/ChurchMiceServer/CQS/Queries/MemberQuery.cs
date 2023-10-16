namespace ChurchMiceServer.CQS.Queries;

public class MemberQuery : IQuery
{
    public int Id { get; private set; }

    public MemberQuery(int id)
    {
        this.Id = id;
    }
}