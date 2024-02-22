namespace Imagine.Core.Entities.Identity;

public class Subscription : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime ExpiresAt { get; set; }
    public Order Order { get; set; }
}
