namespace ChurchMiceServer.CQS.Queries;

public class GetUserQuery : IQuery
{
    public string UserId { get; }

    public GetUserQuery(string userId)
    {
        this.UserId = userId;
    }
}