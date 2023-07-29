using System.ComponentModel.DataAnnotations;

namespace Imagine.Core.Entities.Identity;

public class Subscription : BaseEntity
{
    [Required]
    public string UserId { get; set; }

    public User User { get; set; }
    public DateTime ExpiresAt { get; set; }
}