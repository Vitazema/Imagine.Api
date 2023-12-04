using System.Collections.Concurrent;
using Imagine.Core.Contracts;
using Imagine.Core.Interfaces;

namespace Imagine.Api.Services;

public class TaskProgressService : ITaskProgressService
{
    private readonly ConcurrentDictionary<Guid, AiTask> _taskProgress = new();

    public AiTask GenerateTask(Guid taskId)
    {
        var aiTask = new AiTask()
        {
            Id = taskId,
            Progress = 0,
            Status = AiTaskStatus.Initialized,
        };

        aiTask.OnUpdated += UpdateTask;

        return !_taskProgress.TryAdd(aiTask.Id, aiTask) ? null : aiTask;
    }

    public void UpdateTask(AiTask task)
    {
        _taskProgress.AddOrUpdate(task.Id, task, (id, oldValue) => task);
    }

    public AiTask GetTask(Guid taskId)
    {
        return _taskProgress.TryGetValue(taskId, out var aiTask) ? aiTask : null;
    }

    public void RemoveTask(Guid taskId)
    {
        _taskProgress.TryRemove(taskId, out _);
    }
}
