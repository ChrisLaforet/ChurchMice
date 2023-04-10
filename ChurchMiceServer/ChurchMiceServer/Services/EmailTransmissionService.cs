namespace ChurchMiceServer.Services;

public class EmailTransmissionService : IHostedService, IDisposable
{
    // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio

    private IEmailProxy emailProxy;
    private ILogger<EmailTransmissionService> logger;
    
    public EmailTransmissionService(IEmailProxy emailProxy, ILogger<EmailTransmissionService> logger)
    {
        this.emailProxy = emailProxy;
        this.logger = logger;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}