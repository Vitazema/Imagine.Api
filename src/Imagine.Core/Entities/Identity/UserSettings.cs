using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imagine.Core.Entities.Identity;

public class UserSettings : BaseEntity
{
    [Required]
    public string UserId { get; set; } 
    [ForeignKey("UserId")]
    public User User { get; set; }
    public ArtType SelectedAiType { get; set; }
    public string Language { get; set; }
}
