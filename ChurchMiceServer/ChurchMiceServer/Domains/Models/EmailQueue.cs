namespace ChurchMiceServer.Domains.Models;

public class EmailQueue
{
    public string Id { get; set; }
    public string EmailRecipient { get; set; }
    public string EmailSender { get; set; }
    public string EmailSubject { get; set; }
    public string EmailBody { get; set; }
    public DateTime SentDatetime { get; set; }
    public DateTime? AttemptDatetime { get; set; }
    public int? TotalAttempts { get; set; }
    public string? AttachmentFilename { get; set; }
}
