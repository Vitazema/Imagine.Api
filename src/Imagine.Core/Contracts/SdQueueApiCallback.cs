using System.Text.Json.Serialization;

namespace Imagine.Core.Contracts;

public record SdQueueApiCallback
{
    public string task_id { get; set; }
    [JsonPropertyName("status")]
    public string Status { get; set; }
    [JsonPropertyName("files")]
    public List<string> Files { get; set; }
}
