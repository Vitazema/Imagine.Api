using System.ComponentModel.DataAnnotations;

namespace Imagine.Core.Entities;

public class Art : BaseEntity
{
    public string Title { get; set; }
    public string Url { get; set; }
    public int Progress { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    public User User { get; set; }
    [Required]
    public ArtSettings ArtSettings { get; set; }
}