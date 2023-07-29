using Imagine.Core.Entities;

namespace Imagine.Core.Interfaces;

public interface IArtRepository
{
    Task<Art> GetArtByIdAsync(Guid id);
    Task<IReadOnlyList<Art>> GetArtAsync();
    Task<Guid> DeleteArtAsync(Guid id);
}