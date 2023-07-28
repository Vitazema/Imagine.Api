using System.ComponentModel.DataAnnotations;

namespace Imagine.Core.Entities.Identity;

public class Subscription : BaseEntity
{
    [Required]
    public string Type { get; set; }
    public DateTime ExpiresAt { get; set; }
}