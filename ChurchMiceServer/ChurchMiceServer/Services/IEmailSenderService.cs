namespace ChurchMiceServer.Services;

public interface IEmailSenderService
{
    void SendSingleMessage(string to, string from, string subject, string body);
}