using Imagine.Core.Contracts;
using Imagine.Core.Entities;

namespace Imagine.Core.Interfaces;

public interface IArtStorage
{
    Task<Art> StoreArtAsync(SdQueueTaskResult result, Art art);
}