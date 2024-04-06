using Imagine.Core.Configurations;
using Microsoft.Extensions.Options;

namespace Imagine.Api.Services;

public class WorkerPool : IWorkerPool
{
    private readonly IOptions<WorkersSettings> _workerSettings;

     private static Queue<StableDiffusionWorker> workers = new();

    public WorkerPool(IOptions<WorkersSettings> workerSettings)
    {
        _workerSettings = workerSettings;
        workerSettings.Value.StableDiffusionWorkers.ForEach(w => workers.Enqueue(w));
    }

    public async Task<int> NextWorker()
    {
        var worker = workers.Dequeue();
        if (worker == null) throw new Exception("No workers available");
        workers.Enqueue(worker);
        return worker.Id;
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
        return workers.FirstOrDefault(x => x.Id == id) ??
               throw new Exception($"Error getting worker by id: {id}");
    }
}
