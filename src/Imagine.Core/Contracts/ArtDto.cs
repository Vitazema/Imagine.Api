using Imagine.Core.Entities;

namespace Imagine.Core.Contracts;

public record ArtDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public int Progress { get; set; }
    public DateTime CreatedAt { get; set; }
    public string User { get; set; }
    public string ArtSetting { get; set; }
}
