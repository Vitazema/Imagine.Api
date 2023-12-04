using System.Text.Json.Serialization;

namespace Imagine.Core.Contracts;

public record SdQueueResponse
{
    [JsonPropertyName("task_id")]
    public string TaskId { get; set; }
}
