namespace ChurchMiceServer.Services;

public interface IEmailSenderService
{
    public const string SMTP_SERVER = "SmtpServer";
    public const string SMTP_PORT = "SmtpPort";
    public const string SMTP_USER = "SmtpUser";
    public const string SMTP_PASSWORD = "SmtpPassword";
    public const string SMTP_SENDER = "SmtpSender";
    
    void SendSingleMessage(string to, string from, string subject, string body);
}