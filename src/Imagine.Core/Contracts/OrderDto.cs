using Imagine.Core.Entities;

namespace Imagine.Core.Contracts;

public record OrderDto
{
    public Guid Id { get; set; }
    public SubscriptionDto Subscription { get; set; }
    public int Credentials { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Subtotal { get; set; }
}
