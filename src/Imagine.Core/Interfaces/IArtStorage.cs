using Imagine.Core.Contracts;
using Imagine.Core.Entities;

namespace Imagine.Core.Interfaces;

public interface IArtStorage
{
    Task<Art> StoreArtAsync(SdResponse response, Art art);
}