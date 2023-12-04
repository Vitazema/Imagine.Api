using Imagine.Core.Configurations;
using Imagine.Core.Entities;

namespace Imagine.Api.Services;

public interface IWorkerPool
{
    Task<int> GetWorkerId(Guid taskId);
    Task<string> GetWorkerAddress(Guid taskId);
    StableDiffusionWorker GetWorkerById(int id);
    Task<int> NextWorker();
}
