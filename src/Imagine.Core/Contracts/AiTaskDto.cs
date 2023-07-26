using System.Text.Json.Serialization;

namespace Imagine.Core.Contracts;

public class AiTaskDto
{
    public Guid TaskId { get; init; }
    public int WorkerId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AiTaskStatus Status { get; set; }
    public double Progress { get; set; }
    public double RelativeEstimation { get; set; }
}
