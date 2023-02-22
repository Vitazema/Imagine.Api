using Imagine.Core.Entities;
using Imagine.Infrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace Imagine.Infrastructure.Data;

public class ArtDbContext : DbContext
{
    public ArtDbContext(DbContextOptions<ArtDbContext> options) : base(options)
    {
    }

    public DbSet<Art> Arts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ArtSetting> ArtSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ArtConfiguration());
    }
}