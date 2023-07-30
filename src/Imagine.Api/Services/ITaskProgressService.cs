using Imagine.Core.Contracts;
using Imagine.Core.Entities;

namespace Imagine.Api.Services;

public interface ITaskProgressService
{
    AiTaskDto GenerateTask(Art art);
    void UpdateTask(AiTaskDto task);
    AiTaskDto GetProgress(Guid taskId);
    void RemoveTask(Guid taskId);
}
