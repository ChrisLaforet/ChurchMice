namespace ChurchMiceServer.Services;

// Based on https://learn.microsoft.com/en-us/dotnet/core/extensions/scoped-service

public interface IScopedProcessingService
{
    Task DoWorkAsync(CancellationToken stoppingToken);
}