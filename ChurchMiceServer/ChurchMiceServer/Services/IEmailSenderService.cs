namespace ChurchMiceServer.Services;

public interface IEmailSenderService
{
    void SendSingleMessageTo(string to, string subject, string body);
}