using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.Services;

public sealed class EmailTransmissionService : IScopedProcessingService
{
    public const int DELAY_TIMEOUT_MSEC = 15000;
    
    // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio
    private readonly ILogger<EmailTransmissionService> logger;
    private readonly IEmailProxy emailProxy;
    private readonly IEmailSenderService emailSenderService;

    public EmailTransmissionService(IEmailProxy emailProxy, IEmailSenderService emailSenderService, ILogger<EmailTransmissionService> logger)
    {
        this.emailProxy = emailProxy;
        this.emailSenderService = emailSenderService;
        this.logger = logger;
    }
    
    public async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{ServiceName} working", nameof(EmailTransmissionService));
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!SendNextEmail())
            {
                await Task.Delay(DELAY_TIMEOUT_MSEC, stoppingToken);
            }
        }
    }

    private bool SendNextEmail()
    {
        // read an item that has not been attempted and send it.
        // if nothing, get something attempted over 5 mins ago
        var toSend = emailProxy.GetUnattemptedMessages();
        if (toSend.Count == 0)
        {
            toSend = emailProxy.GetRetryMessages();
        }

        if (toSend.Count > 0)
        {
            var entry = toSend[0];
            try
            {
                emailSenderService.SendSingleMessage(entry.EmailRecipient, entry.EmailSender, entry.EmailSubject, entry.EmailBody);
                emailProxy.DeleteMessage(entry);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Information, "Error sending message " + entry.Id + ": ", ex);
                emailProxy.MarkMessageFailed(entry);
            }

            return true;
        }

        return false;
    }
}