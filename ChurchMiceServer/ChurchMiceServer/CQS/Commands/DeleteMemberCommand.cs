namespace ChurchMiceServer.CQS.Commands;

public class DeleteMemberCommand : ICommand
{
    public int MemberId { get; set; }
    public string CreatorUsername { get; private set; }

    public DeleteMemberCommand(int memberId, string creatorUsername)
    {
        this.MemberId = memberId;
        this.CreatorUsername = creatorUsername;
    }
}