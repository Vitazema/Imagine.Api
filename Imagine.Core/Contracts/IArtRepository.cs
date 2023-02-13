using Imagine.Core.Entities;

namespace Imagine.Core.Contracts;

public interface IArtRepository
{
    Task<Art> GetArtByIdAsync(int id);
    Task<IReadOnlyList<Art>> GetArtAsync();
    Task<ArtSettings> GetArtSettingsByIdAsync(int id);
}