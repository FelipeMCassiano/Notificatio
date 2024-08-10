namespace reciever;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("ServiceWorker finished");
        return base.StopAsync(cancellationToken);
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("ServiceWorker starting");
        return base.StartAsync(cancellationToken);
    }
}
