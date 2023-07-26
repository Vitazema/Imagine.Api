using Imagine.Api.Queue;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;

namespace Imagine.Api.Services;

public class AiService
{
    private readonly ILogger<AiService> _logger;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IAiApiService _aiApiService;
    private readonly ITaskProgressService _taskProgressService;

    public AiService(ILogger<AiService> logger, IBackgroundTaskQueue taskQueue,
        IAiApiService aiApiService, ITaskProgressService taskProgressService)
    {
        _logger = logger;
        _taskQueue = taskQueue;
        _aiApiService = aiApiService;
        _taskProgressService = taskProgressService;
    }

    public async Task<Guid> GenerateAsync(Art art)
    {
        var task = _taskProgressService.GenerateTask(art);
        if (task != null)
        {
            // Queue a background work item
            await _taskQueue.QueueBackgroundWorkItemAsync(async token =>
            {
                var response = await _aiApiService.RequestAsync(art, token);
                if (response is not null)
                {
                    _logger.LogInformation("Response from worker: {Response}", response);
                    // todo: update art entity here
                    // todo: send url images to client
                }
            });
            
            _taskProgressService.UpdateProgress(task.TaskId, new AiTaskDto()
            {
                TaskId = task.TaskId,
                Progress = 0,
                Status = AiTaskStatus.Queued,
                // todo: assign worker from worker pool
                WorkerId = 0
            });
        }

        return art.TaskId;
    }

    // private async ValueTask BuildWorkItemAsync(CancellationToken token)
    // {
    //     return await _sdApiService.RequestAsync(art);


    //     var delayLoop = 0;
    //     
    //     _logger.LogInformation("Queued work item {Guid} is starting.", guid);
    //     
    //     while (!token.IsCancellationRequested && delayLoop < 5)
    //     {
    //         try
    //         {
    // await Task.Delay(1000, token);
    //         }
    //         catch (OperationCanceledException)
    //         {
    //             _logger.LogInformation("Queued work item {Guid} is stopping.", guid);
    //         }
    //         catch (Exception e)
    //         {
    //             Console.WriteLine(e);
    //             throw;
    //         }
    //
    //         ++delayLoop;
    //         
    //         _logger.LogInformation("Queued work item {Guid} is running. {DelayLoop}", guid, delayLoop);
    //     }
    //
    //     var format = delayLoop switch
    //     {
    //         3 => "Queued work item {Guid} is complete.",
    //         _ => "Queued work item {Guid} is cancelled."
    //     };
    //     
    //     _logger.LogInformation(format, guid);
    // }
}
