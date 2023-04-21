using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.Services;

public class EmailTransmissionService : IHostedService
{
    public const int DELAY_TIMEOUT_MSEC = 15000;
    
    // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio

    private readonly IEmailProxy emailProxy;
    private readonly IEmailSenderService emailSenderService;
    private readonly ILogger<EmailTransmissionService> logger;
    private StoppingToken? stoppingToken;
    private Task? workTask;
    
    public EmailTransmissionService(IEmailProxy emailProxy, IEmailSenderService emailSenderService, ILogger<EmailTransmissionService> logger)
    {
        //https://learn.microsoft.com/en-us/dotnet/core/extensions/scoped-service
        this.emailProxy = emailProxy;
        this.emailSenderService = emailSenderService;
        this.logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        this.stoppingToken = new StoppingToken(cancellationToken);
        this.workTask = DoWork();
        await this.workTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (this.stoppingToken != null)
        {
            this.stoppingToken.RequestStop();
        }
    }
    
    private async Task DoWork()
    {
        while (!stoppingToken.IsStopRequested())
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
                }
            }
           else
            {
                await Task.Delay(DELAY_TIMEOUT_MSEC, stoppingToken.CancellationToken);
            }
        }
    }
}

internal class StoppingToken {
    public CancellationToken CancellationToken
    {
        get;
    }
    private bool StopRequested;

    public StoppingToken(CancellationToken cancellationToken)
    {
        this.CancellationToken = cancellationToken;
    }

    public void RequestStop()
    {
        this.StopRequested = true;
    }

    public bool IsStopRequested()
    {
        return this.StopRequested || this.CancellationToken.IsCancellationRequested;
    }
}