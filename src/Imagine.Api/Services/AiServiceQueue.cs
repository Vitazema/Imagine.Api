using Imagine.Api.Queue;

namespace Imagine.Api.Services;

public class AiServiceQueue : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<AiServiceQueue> _logger;
    private readonly IBackgroundTaskQueue _taskQueue;

    public AiServiceQueue(IServiceProvider services, ILogger<AiServiceQueue> logger,
        IBackgroundTaskQueue taskQueue)
    {
        _services = services;
        _logger = logger;
        _taskQueue = taskQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();
            try
            {
                var art = await _taskQueue.DequeueAsync(stoppingToken);

                var aiService = scope.ServiceProvider.GetRequiredService<IAiService>();

                await aiService.GenerateSdIdAsync(stoppingToken, art);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Background task queue is stopping.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing task work item.");
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"{nameof(AiServiceQueue)} is stopping.");

        await base.StopAsync(stoppingToken);
    }
}
