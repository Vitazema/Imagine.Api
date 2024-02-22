using Imagine.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imagine.Infrastructure.Persistence.TypeConfigurations;

public class UserEntityTypeConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // builder.HasOne<UserSettings>()
            // .WithOne(s => s.User)
            // .HasForeignKey<UserSettings>(s => s.UserId)
            // .OnDelete(DeleteBehavior.Cascade);
        // builder.HasMany<Subscription>()
        //     .WithOne(s => s.User)
        //     .HasForeignKey(s => s.UserId);
        // builder.HasMany<Order>()
        //     .WithOne(o => o.User)
        //     .HasForeignKey(o => o.UserId);
        // builder.HasMany<Art>()
        //     .WithOne(a => a.User)
        //     .HasForeignKey(a => a.UserId);
    }
}
