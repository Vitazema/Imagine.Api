using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Imagine.Infrastructure.Persistence;

public class ArtRepository : IArtRepository
{
    private readonly ArtDbContext _context;

    public ArtRepository(ArtDbContext context)
    {
        _context = context;
    }
    public async Task<IReadOnlyList<Art>> GetArtAsync()
    {
        return await _context.Arts
            .Include(a => a.User)
            .ToListAsync();
    }
    public async Task<Art> GetArtByIdAsync(int id)
    {
        return await _context.Arts
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<int> DeleteArtAsync(int id)
    {
        var art = await _context.Arts.FindAsync(id);
        if (art != null) _context.Arts.Remove(art);
        await _context.SaveChangesAsync();
        return id;
    }
}