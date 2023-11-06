using ChurchMiceServer.Types;

namespace ChurchMiceServer.CQS.Queries;

public class GetUserQuery : IQuery
{
    public UserId UserId { get; }

    public GetUserQuery(UserId userId)
    {
        this.UserId = userId;
    }
}