using ChurchMiceServer.Types;

namespace ChurchMiceServer.CQS.Queries;

public class GetMemberImagesQuery : IQuery
{
    public MemberId MemberId { get; private set; }
    public UserId UserId { get; private set; }

    public GetMemberImagesQuery(MemberId memberId, UserId userId)
    {
        MemberId = memberId;
        UserId = userId;
    }
}