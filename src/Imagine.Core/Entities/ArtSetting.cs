using System.Text.Json.Serialization;

namespace Imagine.Core.Entities;

public class ArtSetting : BaseEntity
{
    public string Model { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ArtType Type { get; set; }
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }
    [JsonPropertyName("negative_prompt")]
    public string NegativePrompt { get; set; }
}