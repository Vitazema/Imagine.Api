using Imagine.Core.Contracts;
using Imagine.Core.Entities;

namespace Imagine.Core.Interfaces;

public interface IAiService
{
    Task<AiTask> GenerateSdIdAsync(CancellationToken token, Art art);
    Task StoreArtAsync(SdQueueApiCallback taskId);
}
