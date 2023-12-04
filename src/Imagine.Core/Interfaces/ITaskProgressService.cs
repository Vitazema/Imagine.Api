using Imagine.Core.Contracts;

namespace Imagine.Core.Interfaces;

public interface ITaskProgressService
{
    AiTask GenerateTask(Guid taskId);
    void UpdateTask(AiTask task);
    AiTask GetTask(Guid taskId);
    void RemoveTask(Guid taskId);
}
