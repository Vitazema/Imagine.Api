using Imagine.Core.Entities;

namespace Imagine.Core.Contracts;

public record SubscriptionDto
{
    public string UserName { get; set; }
    public Role Role { get; set; }
    public DateTime ExpiresAt { get; set; }
}
