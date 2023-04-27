using ChurchMiceServer.Domains.Models;

namespace ChurchMiceServer.Domains.Proxies;

public interface IEmailProxy
{
    void SendMessageTo(string to, string from, string subject, string body);
    List<EmailQueue> GetUnattemptedMessages();
    List<EmailQueue> GetRetryMessages();
    void DeleteMessage(EmailQueue entry);
    void MarkMessageFailed(EmailQueue entry);
}