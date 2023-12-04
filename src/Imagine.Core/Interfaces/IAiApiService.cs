using Imagine.Core.Contracts;
using Imagine.Core.Entities;

namespace Imagine.Core.Interfaces;

public interface IAiApiService
{
    Task<string> EnqueueSdTaskAsync(Art art, CancellationToken cancellationToken);
    Task<SdQueueTaskResult> GetSdTaskResultAsync(Art art);
}
