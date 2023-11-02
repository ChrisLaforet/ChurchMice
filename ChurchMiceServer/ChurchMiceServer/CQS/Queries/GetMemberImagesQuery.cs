namespace ChurchMiceServer.CQS.Queries;

public class GetMemberImagesQuery : IQuery
{
    public int MemberId { get; private set; }
    public string UserId { get; private set; }

    public GetMemberImagesQuery(int memberId, string userId)
    {
        MemberId = memberId;
        UserId = userId;
    }
}