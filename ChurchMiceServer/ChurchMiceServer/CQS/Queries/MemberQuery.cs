using ChurchMiceServer.Types;

namespace ChurchMiceServer.CQS.Queries;

public class MemberQuery : IQuery
{
    public MemberId Id { get; private set; }

    public MemberQuery(MemberId id)
    {
        this.Id = id;
    }
}