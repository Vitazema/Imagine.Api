using Imagine.Core.Configurations;
using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;
using Imagine.Infrastructure.Persistence.TypeConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Imagine.Infrastructure.Persistence;

public class ArtDbContext : IdentityDbContext<User, ApplicationRole, Guid>
{
    private readonly IOptions<AppSettings> _appSettings;

    public ArtDbContext(DbContextOptions<ArtDbContext> options, IOptions<AppSettings> appSettings) : base(options)
    {
        _appSettings = appSettings;
    }

    public DbSet<Art> Arts { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Subscription>()
            .HasOne(o => o.Order)
            .WithOne(o => o.Subscription)
            .HasForeignKey<Order>(o => o.SubscriptionId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<UserSettings>()
            .HasOne(u => u.User)
            .WithOne(u => u.UserSettings)
            .HasForeignKey<UserSettings>(us => us.UserId);
        modelBuilder.Entity<UserSettings>()
            .Property(us => us.Language)
            .HasConversion(o => o.ToString(),
                o => (Languages)Enum.Parse(typeof(Languages),
                    o));

        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ArtEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
    }
}
