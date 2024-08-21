namespace reciever;

using reciever.Core.Services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ServiceMsg _service;

    public Worker(ILogger<Worker> logger, ServiceMsg service)
    {
        _logger = logger;
        _service = service;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            try
            {
                await _service.ReceiveMessage(stoppingToken);
                await _service.SendEmails();
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
