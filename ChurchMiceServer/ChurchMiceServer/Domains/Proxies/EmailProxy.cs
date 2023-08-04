using ChurchMiceServer.Domains.Models;

namespace ChurchMiceServer.Domains.Proxies;

public class EmailProxy : IEmailProxy
{
    private readonly IChurchMiceContext context;

    public EmailProxy(IChurchMiceContext context)
    {
        this.context = context;
    }

    public void SendMessageTo(string to, string from, string subject, string body)
    {
        var entry = new EmailQueue();
        entry.Id = Guid.NewGuid().ToString();
        entry.EmailSender = from;
        entry.EmailRecipient = to;
        entry.EmailSubject = subject;
        entry.EmailBody = body;
        entry.SentDatetime = DateTime.Now;
        entry.TotalAttempts = 0;
        context.EmailQueue.Add(entry);
        context.SaveChanges();
    }

    public List<EmailQueue> GetUnattemptedMessages()
    {
        return context.EmailQueue.Where(email => email.TotalAttempts == 0)
            .OrderByDescending(email => email.SentDatetime)
            .ToList();
    }

    public List<EmailQueue> GetRetryMessages()
    {
        var lastAttemptDatetime = DateTime.Now.Subtract(TimeSpan.FromMinutes(5));
        return context.EmailQueue.Where(email => email.AttemptDatetime != null && email.AttemptDatetime <= lastAttemptDatetime)
            .OrderByDescending(email => email.AttemptDatetime)
            .ToList();
    }

    public void DeleteMessage(EmailQueue entry)
    {
        context.EmailQueue.Remove(entry);
        context.SaveChanges();
    }

    public void MarkMessageFailed(EmailQueue entry)
    {
        entry.AttemptDatetime = DateTime.Now;
        entry.TotalAttempts = entry.TotalAttempts == null ? 1 : entry.TotalAttempts + 1;
        context.SaveChanges();
    }
}