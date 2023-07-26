using System.Collections.Concurrent;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;

namespace Imagine.Api.Services;

public class TaskProgressService : ITaskProgressService
{
    private readonly  ConcurrentDictionary<Guid, AiTaskDto> _taskProgress;

    public TaskProgressService()
    {
        _taskProgress = new ConcurrentDictionary<Guid, AiTaskDto>();
    }

    public AiTaskDto GenerateTask(Art art)
    {

            var aiTask = new AiTaskDto()
            {
                Progress = 0,
                Status = AiTaskStatus.Created,
                WorkerId = -1,
                TaskId = Guid.NewGuid(),
            };

            if (!_taskProgress.TryAdd(aiTask.TaskId, aiTask)) return null;
            art.TaskId = aiTask.TaskId;
            return aiTask;
    }

    public void UpdateProgress(Guid taskId, AiTaskDto aiTask)
    {
        _taskProgress.AddOrUpdate(taskId, aiTask, (id, oldValue) => aiTask);
    }

    public AiTaskDto GetProgress(Guid taskId)
    {
        return _taskProgress.TryGetValue(taskId, out var aiTask) ? aiTask : null;
    }

    public void RemoveTask(Guid taskId)
    {
        _taskProgress.TryRemove(taskId, out _);
    }
}
