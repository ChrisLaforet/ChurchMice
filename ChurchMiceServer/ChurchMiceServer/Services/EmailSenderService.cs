﻿using System.Net;
using System.Net.Mail;
using ChurchMiceServer.Configuration;

namespace ChurchMiceServer.Services;

public class EmailSenderService : IEmailSenderService
{
    public const string SMTP_SERVER = "SmtpServer";
    public const string SMTP_PORT = "SmtpPort";
    public const string SMTP_USER = "SmtpUser";
    public const string SMTP_PASSWORD = "SmtpPassword";
    public const string SMTP_SENDER = "SmtpSender";
    
    private readonly IConfigurationLoader configurationLoader;
    private readonly ILogger<EmailSenderService> logger;
    
    public EmailSenderService(IConfigurationLoader configurationLoader, ILogger<EmailSenderService> logger)
    {
        this.configurationLoader = configurationLoader;
        this.logger = logger;
    }

    public void SendSingleMessageTo(string to, string subject, string body)
    {
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls13; 
        var mail = new MailMessage(); 
 
        var sender = configurationLoader.GetKeyValueFor(SMTP_SENDER);
        
        mail.From = new MailAddress(sender);
        mail.To.Add(to);
        mail.Subject = subject; 
        mail.Body = body; 
        
        var smtp = new SmtpClient(configurationLoader.GetKeyValueFor(SMTP_SERVER));
        var port = int.Parse(configurationLoader.GetKeyValueFor(SMTP_PORT));
        
        var credentials = new NetworkCredential(configurationLoader.GetKeyValueFor(SMTP_USER), configurationLoader.GetKeyValueFor(SMTP_PASSWORD)); 
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = credentials;
        smtp.Port = port;
        smtp.EnableSsl = false;
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.Send(mail);
        //smtp.SendAsync(mail, "d843cb13-fa54-427e-bb18-dafe5cf0ae24"); 
    }
}