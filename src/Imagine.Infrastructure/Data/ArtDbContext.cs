using Imagine.Core.Entities;
using Imagine.Infrastructure.Data.Configurations;
using Imagine.Infrastructure.Data.TypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Imagine.Infrastructure.Data;

public class ArtDbContext : DbContext
{
    private readonly IOptions<AppSettings> _appSettings;

    public ArtDbContext(DbContextOptions<ArtDbContext> options, IOptions<AppSettings> appSettings) : base(options)
    {
        _appSettings = appSettings;
    }

    public DbSet<Art> Arts { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ArtEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration(_appSettings));
    }
}