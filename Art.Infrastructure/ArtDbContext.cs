using Art.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Art.Infrastructure;

public class ArtDbContext : DbContext
{
    public ArtDbContext(DbContextOptions<ArtDbContext> options) : base(options)
    {
    }

    public DbSet<Avatar> Avatars { get; set; }
}