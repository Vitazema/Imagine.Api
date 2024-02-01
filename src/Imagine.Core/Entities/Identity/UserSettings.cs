using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imagine.Core.Entities.Identity;

public class UserSettings : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ArtType SelectedAiType { get; set; }
    public Languages Language { get; set; }
}
