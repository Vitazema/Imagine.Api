using Microsoft.AspNetCore.Identity;

namespace Imagine.Core.Entities.Identity;

public class User : IdentityUser
{
    public Role Role { get; set; }
    public Subscription Subscription { get; set; }
    public ICollection<Art> Arts { get; set; }
}