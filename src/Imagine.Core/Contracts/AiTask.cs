using System.Text.Json.Serialization;

namespace Imagine.Core.Contracts;

public class AiTask
{
    public event Action<AiTask> OnUpdated;
    
    private AiTaskStatus _status;
    public Guid Id { get; init; }
    public int WorkerId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AiTaskStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            // OnUpdated?.Invoke(this);
        }
    }
    public double? Progress { get; set; }
    public double? RelativeEstimation { get; set; }
    public bool Completed { get; set; }
}
