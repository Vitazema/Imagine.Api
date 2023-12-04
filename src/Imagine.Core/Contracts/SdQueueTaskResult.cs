using System.Text.Json.Serialization;

namespace Imagine.Core.Contracts;

public record SdQueueTaskResult
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    [JsonPropertyName("data")]
    public List<SdQueueResultData> Data { get; set; }
}

public record SdQueueResultData
{
    [JsonPropertyName("image")]
    public string Image { get; set; }
    [JsonPropertyName("infotext")]
    public string InfoText { get; set; }
}

