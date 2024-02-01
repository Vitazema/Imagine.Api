using Imagine.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imagine.Infrastructure.Persistence.TypeConfigurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasOne(o => o.Subscription)
            .WithOne(o => o.Order);
        builder.Property(o => o.Status)
            .HasConversion(o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus),
                    o));
        builder.Property(o => o.Subtotal)
            .HasColumnType("decimal(18,2)");
    }
}
