using System.Net;
using System.Net.Mail;
using ChurchMiceServer.Configuration;

namespace ChurchMiceServer.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly IConfigurationLoader configurationLoader;
    private readonly ILogger<EmailSenderService> logger;
    
    public EmailSenderService(IConfigurationLoader configurationLoader, ILogger<EmailSenderService> logger)
    {
        this.configurationLoader = configurationLoader;
        this.logger = logger;
    }

    public void SendSingleMessage(string to, string from, string subject, string body)
    {
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls13; 
        var mail = new MailMessage(); 
 
        logger.LogInformation($"Sending email to {to} from {from} with subject {subject}");
//        var sender = configurationLoader.GetKeyValueFor(IEmailSenderService.SMTP_SENDER);
        
        mail.From = new MailAddress(from);
        mail.To.Add(to);
        mail.Subject = subject; 
        mail.Body = body; 
        
        var smtp = new SmtpClient(configurationLoader.GetKeyValueFor(IEmailSenderService.SMTP_SERVER));
        var port = int.Parse(configurationLoader.GetKeyValueFor(IEmailSenderService.SMTP_PORT));
        
        var credentials = new NetworkCredential(configurationLoader.GetKeyValueFor(IEmailSenderService.SMTP_USER), 
            configurationLoader.GetKeyValueFor(IEmailSenderService.SMTP_PASSWORD)); 
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = credentials;
        smtp.Port = port;
        smtp.EnableSsl = false;
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.Send(mail);
        //smtp.SendAsync(mail, "d843cb13-fa54-427e-bb18-dafe5cf0ae24"); 
        logger.LogInformation($"Sent email to {to} from {from} with subject {subject}");

    }
}