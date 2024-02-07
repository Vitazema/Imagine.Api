namespace Imagine.Api.Queue;

public interface IBackgroundTaskQueue
{
    ValueTask EnqueueAsync(Art art);
    ValueTask<Art> DequeueAsync(CancellationToken cancellationToken);
}
