using Imagine.Core.Configurations;
using Imagine.Core.Contracts;
using Imagine.Core.Contracts.Errors;
using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;
using Imagine.Core.Interfaces;
using Imagine.Core.Models;
using Imagine.Core.Specifications;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

namespace Imagine.Api.Services;

public class AiService : IAiService
{
    private readonly AppSettings _appSettings;
    private readonly IUnitOfWork _iUnitOfWork;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IAiApiService _aiApiService;
    private readonly ITaskProgressService _taskProgressService;
    private readonly IArtStorage _artStorage;
    private readonly IRepository<Art> _artsRepository;
    private readonly IWorkerPool _workerPool;

    public AiService(IOptions<AppSettings> appSettings, IAiApiService aiApiService,
        ITaskProgressService taskProgressService, IArtStorage artStorage,
        IRepository<Art> artsRepository, IWorkerPool workerPool, IUnitOfWork iUnitOfWork,
        IPermissionRepository permissionRepository)
    {
        _appSettings = appSettings.Value;
        _aiApiService = aiApiService;
        _taskProgressService = taskProgressService;
        _artStorage = artStorage;
        _artsRepository = artsRepository;
        _workerPool = workerPool;
        _iUnitOfWork = iUnitOfWork;
        _permissionRepository = permissionRepository;
    }


    public async Task<Art> CreateArtAsync(User user, ArtDto artDto)
    {
        var art = new Art()
        {
            User = user,
            ArtSetting = artDto.ArtSetting.ToJsonString(),
            Type = artDto.ArtType,
            Title = artDto.Title
        };
        // var task = _taskProgressService.GenerateTask(newArt);

        // Queue a background work item
        // await _taskQueue.EnqueueAsync(newArt);
        var task = await GenerateSdIdAsync(CancellationToken.None, art);
        if (task is null || task.Id == Guid.Empty)
            throw new Exception("Failed to generate task id");

        _iUnitOfWork.Repository<Art>().Add(art);
        var result = await _iUnitOfWork.Complete();
        if (result <= 0)
        {
            return null;
        }

        await _permissionRepository.EditCredentialsAsync(user.UserName, -10);
        return art;
    }

    public async Task<AiTask> GenerateSdIdAsync(CancellationToken token, Art art)
    {
        // Inject SD queue api callback
        art.WorkerId = await _workerPool.NextWorker();
        art.SetArtSetting("callback_url", $"{_appSettings.HostAddress}/progress/callback");
        art.SetArtSetting("steps", "25");
        art.SetArtSetting("negative_prompt", "ugly blurry text");

        var taskId = await _aiApiService.EnqueueSdTaskAsync(art, token);
        if (taskId is null) return null;

        art.Id = new Guid(taskId);
        var task = _taskProgressService.GenerateTask(art.Id);
        task.Status = AiTaskStatus.Queued;
        task.WorkerId = art.WorkerId;

        return task;
    }

    public async Task StoreArtAsync(SdQueueApiCallback callback)
    {
        var specification = new ArtsWithUserAndTypeSpecification(new Guid(callback.task_id));
        var art = await _artsRepository.GetEntityWithSpec(specification);
        var task = _taskProgressService.GetTask(art.Id);

        var taskResult = await _aiApiService.GetSdTaskResultAsync(art);

        var updatedArt = await _artStorage.StoreArtAsync(taskResult, art);

        var res = await _artsRepository.UpdateAsync(updatedArt);
        task.Status = AiTaskStatus.Completed;
        task.Progress = 100;
        task.RelativeEstimation = 0;
    }
}
