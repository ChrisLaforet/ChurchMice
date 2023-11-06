using ChurchMiceServer.Types;

namespace ChurchMiceServer.CQS.Commands;

public class DeleteMemberCommand : ICommand
{
    public MemberId MemberId { get; set; }
    public string CreatorUsername { get; private set; }

    public DeleteMemberCommand(MemberId memberId, string creatorUsername)
    {
        this.MemberId = memberId;
        this.CreatorUsername = creatorUsername;
    }
}