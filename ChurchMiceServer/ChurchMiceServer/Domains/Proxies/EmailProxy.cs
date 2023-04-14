using ChurchMiceServer.Configuration;
using ChurchMiceServer.Services;

namespace ChurchMiceServer.Domains.Proxies;

public class EmailProxy : IEmailProxy
{
    private readonly IEmailSenderService emailSenderService;

    public EmailProxy(IServiceProvider serviceProvider, IConfigurationLoader configurationLoader)
    {
        emailSenderService = ActivatorUtilities.CreateInstance<EmailSenderService>(serviceProvider);
    }

    public void SendMessageTo(string to, string subject, string body)
    {
        emailSenderService.SendSingleMessageTo(to, subject, body);
    }
}