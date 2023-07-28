using Imagine.Api.Queue;

namespace Imagine.Api.Services;

#nullable enable
public class AiServiceQueue : BackgroundService
{
    private readonly ILogger<AiServiceQueue> _logger;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly IBackgroundTaskQueue _taskQueue;

    public AiServiceQueue(ILogger<AiServiceQueue> logger, IHostApplicationLifetime lifetime,
        IBackgroundTaskQueue taskQueue)
    {
        _logger = logger;
        _lifetime = lifetime;
        _taskQueue = taskQueue;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return ProcessTaskQueueAsync(stoppingToken);
    }

    private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                Func<CancellationToken, ValueTask>? workItem = await _taskQueue.DequeueAsync(stoppingToken);
                await workItem(stoppingToken);
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
