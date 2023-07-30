using Imagine.Api.Queue;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Imagine.Core.Interfaces;

namespace Imagine.Api.Services;

public class AiService : IAiService
{
    private readonly IAiApiService _aiApiService;
    private readonly ITaskProgressService _taskProgressService;
    private readonly IArtStorage _artStorage;
    private readonly IRepository<Art> _artsRepository;

    public AiService(IAiApiService aiApiService, ITaskProgressService taskProgressService, IArtStorage artStorage,
        IRepository<Art> artsRepository
    )
    {
        _aiApiService = aiApiService;
        _taskProgressService = taskProgressService;
        _artStorage = artStorage;
        _artsRepository = artsRepository;
    }

    public async Task GenerateAsync(CancellationToken token, Art art)
    {
        var response = await _aiApiService.RequestAsync(art, token);
        if (response is not null)
        {
            var updatedArt = await _artStorage.StoreArtAsync(response, art);
            var task = _taskProgressService.GetProgress(art.TaskId);
            task.Progress = 100;
            task.Status = AiTaskStatus.Completed;
            _taskProgressService.UpdateTask(task);

            // todo: update art entity here
            var res = await _artsRepository.UpdateAsync(updatedArt);
            // todo: send url images to client
        }
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
