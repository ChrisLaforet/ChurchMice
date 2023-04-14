using ChurchMiceServer.Domains.Proxies;

namespace ChurchMiceServer.Services;

public class EmailTransmissionService : IHostedService
{
    public const int DELAY_TIMEOUT_MSEC = 15000;
    
    // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio

    private IEmailProxy emailProxy;
    private ILogger<EmailTransmissionService> logger;
    private StoppingToken? stoppingToken;
    private Task? workTask;
    
    public EmailTransmissionService(IEmailProxy emailProxy, ILogger<EmailTransmissionService> logger)
    {
        this.emailProxy = emailProxy;
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
            bool didWork = false;
            
            // read an item that has not been attempted and send it.
            // if nothing, get something attempted over 5 mins ago
            emailProxy.SendMessageTo();
            
executionCount++;

_logger.LogInformation(
    "Scoped Processing Service is working. Count: {Count}", executionCount);

            if (!didWork)
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