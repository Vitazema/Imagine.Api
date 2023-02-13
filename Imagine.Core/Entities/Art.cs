using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Imagine.Core.Entities;

public class Art : BaseEntity
{
    public string Title { get; set; }
    public string Url { get; set; }
    public int Progress { get; set; }
    public User User { get; set; }
    [Required]
    public ArtSettings ArtSettings { get; set; }
}