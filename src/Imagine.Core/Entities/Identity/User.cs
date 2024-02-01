using Microsoft.AspNetCore.Identity;

namespace Imagine.Core.Entities.Identity;

public class User : IdentityUser<Guid>
{
    public Role Role { get; set; }
    public UserSettings UserSettings { get; set; }
    public ICollection<Art> Arts { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
    public ICollection<Order> Orders { get; set; }
}
