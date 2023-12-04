using Imagine.Core.Configurations;
using Imagine.Core.Entities;
using Microsoft.Extensions.Options;

namespace Imagine.Api.Services;

public class WorkerPool : IWorkerPool
{
    private readonly IOptions<WorkersSettings> _workerSettings;

    public WorkerPool(IOptions<WorkersSettings> workerSettings)
    {
        _workerSettings = workerSettings;
    }

    public async Task<int> NextWorker()
    {
        var workerId = _workerSettings.Value.StableDiffusionWorkers.FirstOrDefault()?.Id;
        if (workerId is null) throw new Exception("No workers available");
        return workerId.Value;
    }

    public async Task<int> GetWorkerId(Guid taskId)
    {
        var workerId = _workerSettings.Value.StableDiffusionWorkers.FirstOrDefault()?.Id;
        if (workerId is null) throw new Exception("No workers available");
        return workerId.Value;
    }

    public async Task<string> GetWorkerAddress(Guid taskId)
    {
        var workerAddress = _workerSettings.Value.StableDiffusionWorkers.FirstOrDefault()?.Address;
        if (workerAddress is null) throw new Exception("No workers available");
        return workerAddress;
    }

    public StableDiffusionWorker GetWorkerById(int id)
    {
        return _workerSettings.Value.StableDiffusionWorkers.FirstOrDefault(x => x.Id == id) ??
               throw new Exception($"Error getting worker by id: {id}");
    }
}
