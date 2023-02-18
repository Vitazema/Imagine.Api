using Imagine.Core.Entities;

namespace Imagine.Api.Dtos;

public class ArtResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public int Progress { get; set; }
    public string CreatedAt { get; set; }
    public string User { get; set; }
    public ArtSettings ArtSettings { get; set; }
}