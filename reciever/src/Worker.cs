namespace reciever;

using reciever.Core.Services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {

        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var scope = _serviceProvider.CreateScope();

                var _serviceMsg = scope.ServiceProvider.GetRequiredService<ServiceMsg>();
                await _serviceMsg.ReceiveMessage(stoppingToken);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email at {time}", DateTime.Now);
            }


            await Task.Delay(6000, stoppingToken);
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
