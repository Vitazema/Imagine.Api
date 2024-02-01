using System.Text.Json.Serialization;
using Imagine.Core.Entities;

namespace Imagine.Core.Contracts;

public record UserSettingsDto
{
    [JsonPropertyName("selectedFeature")]
    public ArtType AiType { get; set; }
    public Languages Language { get; set; }
}
