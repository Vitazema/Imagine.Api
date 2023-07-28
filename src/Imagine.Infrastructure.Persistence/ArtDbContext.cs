using Imagine.Core.Configurations;
using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;
using Imagine.Infrastructure.Persistence.TypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Imagine.Infrastructure.Persistence;

public class ArtDbContext : DbContext
{
    private readonly IOptions<AppSettings> _appSettings;

    public ArtDbContext(DbContextOptions<ArtDbContext> options, IOptions<AppSettings> appSettings) : base(options)
    {
        _appSettings = appSettings;
    }

    public DbSet<Art> Arts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ArtEntityTypeConfiguration());
    }
}