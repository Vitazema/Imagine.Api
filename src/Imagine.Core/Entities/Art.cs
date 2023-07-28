using System.Text.Json.Serialization;
using Imagine.Core.Entities.Identity;

namespace Imagine.Core.Entities;

public class Art : BaseEntity
{
    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public int Progress { get; set; }
    public User User { get; set; }
    public string Model { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ArtType Type { get; set; }
    public string Prompt { get; set; }
    public string NegativePrompt { get; set; }
    public string ArtSetting { get; set; }
}