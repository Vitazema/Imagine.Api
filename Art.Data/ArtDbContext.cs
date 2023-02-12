using Art.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Art.Data;

public class ArtDbContext : DbContext
{
    public ArtDbContext(DbContextOptions<ArtDbContext> options) : base(options)
    {
        
    }

    public DbSet<Avatar> Avatars { get; set; }
}