
using System.Text.Json.Serialization;

namespace Imagine.Core.Contracts;

public record SdTaskProgressDto
{
    [JsonPropertyName("active")]
    public bool Active { get; set; }
    [JsonPropertyName("queued")]
    public bool Queued { get; set; }
    [JsonPropertyName("completed")]
    public bool Completed { get; set; }
    [JsonPropertyName("progress")]
    public double? Progress { get; set; }
    [JsonPropertyName("eta")]
    public double? Eta { get; set; }
    [JsonPropertyName("live_preview")]
    public string LivePreview { get; set; }
    [JsonPropertyName("id_live_preview")]
    public int IdLivePreview { get; set; }
    [JsonPropertyName("textinfo")]
    public string TextInfo { get; set; }
    
}
