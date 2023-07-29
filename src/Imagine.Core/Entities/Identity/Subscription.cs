using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imagine.Core.Entities.Identity;

public class Subscription : BaseEntity
{
    [Required]
    public string UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
    public DateTime ExpiresAt { get; set; }
}