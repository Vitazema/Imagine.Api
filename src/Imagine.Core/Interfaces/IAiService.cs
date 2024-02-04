using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;

namespace Imagine.Core.Interfaces;

public interface IAiService
{
    Task<Art> CreateArtAsync(User user, ArtDto art);
    Task<AiTask> GenerateSdIdAsync(CancellationToken token, Art art);
    Task StoreArtAsync(SdQueueApiCallback taskId);
}
